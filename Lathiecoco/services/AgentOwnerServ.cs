﻿using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

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
                rp.Code = 400;
                rp.Msg = ex.Message;
            }
              return rp;
        }
        public async Task<ResponseBody<OwnerAgent>> addOwnerAgent(BodyOwnerAgentDto oa)
        {

            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent oaObj=await _CatalogDbContext.OwnerAgents.Where(x => x.Login == oa.Login || x.Email==oa.Email).FirstOrDefaultAsync();
                if(oaObj != null)
                {
                    rp.Msg = "Staff Already exist";
                    rp.IsError = true;
                    rp.Code = 350;
                    return rp;
                }
                OwnerAgent ownerAgent = new OwnerAgent();
                ownerAgent.IdOwnerAgent=Ulid.NewUlid();
                ownerAgent.FirstName = oa.FirstName.Trim().Replace(" ", "");
                ownerAgent.MiddleName = oa.MiddleName.Trim().Replace(" ", "");
                ownerAgent.LastName = oa.LastName.Trim().Replace(" ", "");
                ownerAgent.Phone = oa.Phone.Trim().Replace(" ", "");
                ownerAgent.Address = oa.Address.Trim().Replace(" ", ""); 
                ownerAgent.Country = "Guinée";
                ownerAgent.Email = oa.Email.Trim().Replace(" ", "");
                ownerAgent.Login=oa.Login.Trim().Replace(" ", "");
                ownerAgent.Password= BCrypt.Net.BCrypt.EnhancedHashPassword(oa.Password.Trim().Replace(" ", ""), 15);
                ownerAgent.Profil=oa.Profil.Trim().Replace(" ", "");
                ownerAgent.Address=oa.Address.Trim().Replace(" ", "");
                ownerAgent.IsActive=true;
                ownerAgent.IsFirstLogin=true;
                ownerAgent.AgentType=oa.AgentType.Trim().Replace(" ", "");
                ownerAgent.CreatedDate=DateTime.UtcNow;
                ownerAgent.UpdatedDate=DateTime.UtcNow;
                ownerAgent.CodeOwnerAgent = "N" + GlobalFunction.ConvertToUnixTimestamp(DateTime.UtcNow);
                await _CatalogDbContext.OwnerAgents.AddAsync(ownerAgent);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = ownerAgent;

            }
            catch (Exception ex)
            {
                rp.Code = 400;
                rp.IsError = true;
                rp.Msg = ex.Message.ToString();
                Console.WriteLine(ex.ToString());
            }
            return rp;
        }

        public async Task<ResponseBody<OwnerAgent>> updatePassword(ChangePasswordDto cp)
        {
            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent st = await _CatalogDbContext.OwnerAgents.FindAsync(cp.Id);
                if (st != null)
                {
                    if (BCrypt.Net.BCrypt.EnhancedVerify(cp.OldPassword, st.Password))
                    {
                        st.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(cp.NewPassword.Trim().Replace(" ", ""), 15);
                        st.UpdatedDate = DateTime.UtcNow;
                        //agency.fkIdStaff = ag.fkIdStaff;
                        st.IsFirstLogin = false;
                        _CatalogDbContext.OwnerAgents.Update(st);
                        await _CatalogDbContext.SaveChangesAsync();
                        
                    }
                    else
                    {
                        rp.Msg = "Old password not match!";
                        rp.IsError = true;
                        rp.Code = 010;
                    }
                }
                else
                {
                    rp.Msg = "OwnerAgent not found!";
                    rp.IsError = true;
                    rp.Code = 103;
                }

                rp.Body = st;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<OwnerAgent>> updateOwnerAgent(BodyAgentOwnerUpdateDto oa,Ulid idOwnerAgent)
        {

            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent ownerAgent = await _CatalogDbContext.OwnerAgents.Where(x => x.IdOwnerAgent==idOwnerAgent).FirstOrDefaultAsync();
                if (ownerAgent == null)
                {
                    rp.Msg = "Staff not found!";
                    rp.IsError = true;
                    rp.Code = 404;
                    return rp;
                }
                ownerAgent.FirstName = oa.FirstName.Trim().Replace(" ", ""); ;
                ownerAgent.MiddleName = oa.MiddleName.Trim().Replace(" ", ""); ;
                ownerAgent.LastName = oa.LastName.Trim().Replace(" ", ""); ;
                ownerAgent.Phone = oa.Phone.Trim().Replace(" ", ""); ;
                ownerAgent.Address = oa.Address;
                ownerAgent.Email = oa.Email;
                ownerAgent.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(oa.Password.Trim().Replace(" ",""));
                ownerAgent.Profil = oa.Profil;
                ownerAgent.Address = oa.Address;
                ownerAgent.IsActive = true;
                ownerAgent.IsFirstLogin = true;
                ownerAgent.UpdatedDate = DateTime.UtcNow;
                
                _CatalogDbContext.OwnerAgents.Update(ownerAgent);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = ownerAgent;

            }
            catch (Exception ex)
            {
                rp.Code = 400;
                rp.IsError = true;
                rp.Msg = ex.Message.ToString();
                Console.WriteLine(ex.ToString());
            }
            return rp;
        }

        public async Task<ResponseBody<List<OwnerAgent>>> ownerAgentSearch(string? email, string? phone, int page = 1, int limit = 10)
        {
            string sql = "select * from \"OwnerAgents\" where \"IsActive\"=true";


            if (email != null)
            {
                sql += " and \"Email\" LIKE '%" + email + "%'";
            }
            if (phone != null)
            {
                sql += " and \"Phone\" LIKE '%" + phone + "%'";
            }
            //sql += ";";
            Console.WriteLine(sql);
            ResponseBody<List<OwnerAgent>> rp = new ResponseBody<List<OwnerAgent>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.OwnerAgents != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.OwnerAgents.FromSqlRaw(sql).Count() / limit);

                    var ps = await _CatalogDbContext.OwnerAgents.FromSqlRaw(sql).OrderByDescending(c => c.UpdatedDate).Skip(skip).Take(limit).ToListAsync();

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
                rp.Code = 400;

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
                    rp.Code = 400;
                    return rp;
                }
                

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.Message.ToString();
                rp.Code = 400;
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
                        rp.Code = 322;
                        return rp;
                    }
                    if (!user.IsActive)
                    {
                        rp.IsError = true;
                        rp.Msg = "Your account not active";
                        rp.Code = 320;
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

                        //_ca.HttpContext.Response.Cookies.Append("token", mtoken);
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
                    rp.Code = 330;
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

        public async Task<ResponseBody<OwnerAgent>> blockOrDeblockOwnerAgent(ActiveBlockDto dto)
        {
            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent agu = await _CatalogDbContext.OwnerAgents.Where(a => a.IdOwnerAgent == dto.IdUser).FirstOrDefaultAsync();
            if (agu != null)
            {

                agu.IsBlocked = !agu.IsBlocked;
                //agu.FkIdStaff = dto.FkIdStaff;
                agu.UpdatedDate = DateTime.UtcNow;
                _CatalogDbContext.OwnerAgents.Update(agu);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = agu;
                return rp;

            }
            else
            {
                rp.IsError = true;
                rp.Msg = "Agency user not found";
                rp.Code = 350;
                return rp;
            }

        }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Code = 400;
                rp.Msg = ex.Message;
            }
            return rp;
        }

        public async Task<ResponseBody<OwnerAgent>> activateOrDeactiveOwnerAgent(ActiveBlockDto dto)
        {
            ResponseBody<OwnerAgent> rp = new ResponseBody<OwnerAgent>();
            try
            {
                OwnerAgent agu = await _CatalogDbContext.OwnerAgents.Where(a => a.IdOwnerAgent == dto.IdUser).FirstOrDefaultAsync();
                if (agu != null)
                {

                    agu.IsActive = !agu.IsActive;
                    //agu.FkIdStaff = dto.FkIdStaff;
                    agu.UpdatedDate = DateTime.UtcNow;
                    _CatalogDbContext.OwnerAgents.Update(agu);
                    await _CatalogDbContext.SaveChangesAsync();
                    rp.Body = agu;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Msg = "Agency user not found";
                    rp.Code = 350;
                    return rp;
                }

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Code = 400;
                rp.Msg = ex.Message;
            }
            return rp;
        }
    }
   

}
