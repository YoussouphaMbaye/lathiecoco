using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace  Lathiecoco.services
{
    public class AgentOwnerServ : AgentOwnerRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly IConfiguration _configuration;
        public AgentOwnerServ(CatalogDbContext catalogDbContext, IConfiguration configuration)
        {
            _CatalogDbContext = catalogDbContext;
            _configuration = configuration;
        }
        public async Task<ResponseBody<BodyNbCountDto>> getStatistique()
        {
            ResponseBody<BodyNbCountDto> rp = new ResponseBody<BodyNbCountDto>();
            try
            {
                double nbStaff = await _CatalogDbContext.OwnerAgents.CountAsync();
                double nbAgent = await _CatalogDbContext.CustomerWallets.Where(x => x.Profile == "AGENT").CountAsync();
                double nbCustomer = await _CatalogDbContext.CustomerWallets.Where(x => x.Profile == "CUSTOMER").CountAsync();
                BodyNbCountDto bodyNbCountDto = new BodyNbCountDto();
                bodyNbCountDto.NbStaff = nbStaff;
                bodyNbCountDto.NbPartner = 0;
                bodyNbCountDto.NbCustomer= nbCustomer;
                bodyNbCountDto.NbAgent= nbAgent;
                rp.Body= bodyNbCountDto;
                return rp;
            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Body = null;
                rp.Msg = ex.Message;
            }
              return rp;
        }
        public async Task<ResponseBody<OwnerAgent>> addOwnerAgent(BodyOwnerAgentDto oa)
        {

            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent oaObj=await _CatalogDbContext.OwnerAgents.Where(x => x.Login == oa.Login).FirstOrDefaultAsync();
                if(oaObj != null)
                {
                    rp.Msg = "Staff Already exist";
                    rp.IsError = true;
                    return rp;
                }
                OwnerAgent ownerAgent = new OwnerAgent();
                ownerAgent.IdOwnerAgent=Ulid.NewUlid();
                ownerAgent.FirstName = oa.FirstName;
                ownerAgent.MiddleName = oa.MiddleName;
                ownerAgent.LastName = oa.LastName;
                ownerAgent.Phone = oa.Phone;
                ownerAgent.Address = oa.Address;
                ownerAgent.Country = "Guinée";
                ownerAgent.Email = oa.Email;
                ownerAgent.Login=oa.Login;
                ownerAgent.Password=oa.Password;
                ownerAgent.Profil=oa.Profil;
                ownerAgent.Address=oa.Address;
                ownerAgent.IsActive=true;
                ownerAgent.AgentType=oa.AgentType;
                ownerAgent.CreatedDate=DateTime.Now;
                ownerAgent.UpdatedDate=DateTime.Now;
                ownerAgent.CodeOwnerAgent = "N" + GlobalFunction.ConvertToUnixTimestamp(DateTime.Now);
                await _CatalogDbContext.OwnerAgents.AddAsync(ownerAgent);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = ownerAgent;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = "error";
                Console.WriteLine(ex.ToString());
            }
            return rp;
        }

        public async Task<ResponseBody<List<OwnerAgent>>> findAllOwnerAgents(int page = 1, int limit = 10)
        {
            ResponseBody<List<OwnerAgent>> rp = new ResponseBody<List<OwnerAgent>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.OwnerAgents != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.OwnerAgents.Count() / limit);
                    var ps = await _CatalogDbContext.OwnerAgents.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<OwnerAgent>();
                    }
                }

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;

            }
            return rp;
        }


        public async Task<ResponseBody<OwnerAgent>> findOwnerAgenctById(Ulid Id)
        {
            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {

                OwnerAgent owa = await _CatalogDbContext.OwnerAgents.FindAsync(Id);
                await _CatalogDbContext.SaveChangesAsync();
                if(owa != null)
                {
                    rp.Body = owa;
                    return rp;
                    
                }else
                {
                    rp.IsError= true;
                    rp.Body = null;
                    rp.Msg = "Staff not found";
                    return rp;
                }
                

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = "error";
            }
            return rp;
        }
        public async Task<ResponseBody<OwnerAgent>> login(LoginDto bd)
        {
            string mtoken = "";
            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent user = await _CatalogDbContext.OwnerAgents.Where(s => s.Login == bd.username).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (user.IsBlocked)
                    {
                        rp.IsError = true;
                        rp.Msg = "Your account is blocked";
                        return rp;
                    }
                    if (!user.IsActive)
                    {
                        rp.IsError = true;
                        rp.Msg = "Your account not active";
                        return rp;
                    }
                    //if (BCrypt.Net.BCrypt.EnhancedVerify(us.password, user.Password))
                    if (bd.password == user.Password)
                    {
                        //var token = await BuildToken(userInfo, new[] { RoleTypes.User });
                        var claims = new List<Claim>() {
                          new Claim("Username", user.Login),
                          new Claim("id",user.IdOwnerAgent.ToString()),
                          new Claim(ClaimTypes.Role, user.Profil)

                        };
                       

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));

                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var expiration = DateTime.UtcNow.AddHours(1);
                        JwtSecurityToken token = new JwtSecurityToken(
                           issuer: null,
                           audience: null,
                           claims: claims,
                           expires: expiration,
                           signingCredentials: creds
                        );


                        mtoken = new JwtSecurityTokenHandler().WriteToken(token);
                        
                        if (user.LoginCount > 1)
                        {
                            user.LoginCount = 0;
                            _CatalogDbContext.OwnerAgents.Update(user);
                            await _CatalogDbContext.SaveChangesAsync();
                        }

                        _CatalogDbContext.OwnerAgents.Update(user);
                        await _CatalogDbContext.SaveChangesAsync();

                        //HttpContext.Response.Cookies.Append("token", mtoken);
                        rp.Body = user;

                    }
                    else
                    {
                        user.LoginCount = user.LoginCount + 1;
                        _CatalogDbContext.OwnerAgents.Update(user);
                        await _CatalogDbContext.SaveChangesAsync();
                        if (user.LoginCount > 5)
                        {
                            user.IsBlocked= true;
                            _CatalogDbContext.OwnerAgents.Update(user);
                            await _CatalogDbContext.SaveChangesAsync();
                        }
                        rp.Msg = "Login or password incorrect";
                        rp.IsError = true;
                        return rp;
                        
                        //return BadRequest();

                    }

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Login or password incorrect";
                    return rp;
                    //throw new Exception("Login or password incorrect");
                }

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
                rp.Body = null;

            }
            return rp;

        }
    }
   

}
