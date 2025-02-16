
using apimoney.services;
using Lathiecoco.dto;
using Lathiecoco.models;
using Lathiecoco.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Lathiecoco.services
{
    public class AgencyUserService : AgencyUserRep
    {
        private readonly CatalogDbContext _CatalogDbContext;
        private readonly IConfiguration _configuration;
        public AgencyUserService(CatalogDbContext catalogDbContext, IConfiguration configuration)
        {
            _CatalogDbContext = catalogDbContext;
            _configuration = configuration;
        }

        public async Task<ResponseBody<AgencyUser>> addAgencyUser(BodyAgencyUserDto oa)
        {

            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {
                AgencyUser oaObj = await _CatalogDbContext.AgencyUsers.Where(x => x.Login == oa.Login).FirstOrDefaultAsync();
                if (oaObj != null)
                {
                    rp.Msg = "User Already exist";
                    rp.IsError = true;
                    rp.Code = 350;
                    return rp;
                }

                AgencyUser AgencyUser = new AgencyUser();
                AgencyUser.IdAgencyUser = Ulid.NewUlid();
                AgencyUser.FkIdStaff = oa.FkidStaff;
                AgencyUser.FkIdAgency = oa.FkidAgency;
                AgencyUser.FirstName = oa.FirstName.Trim().Replace(" ", "");
                AgencyUser.MiddleName = oa.MiddleName.Trim().Replace(" ", "");
                AgencyUser.LastName = oa.LastName.Trim().Replace(" ", "");
                AgencyUser.Phone = oa.Phone.Trim().Replace(" ", "");
                AgencyUser.Address = oa.Address.Trim().Replace(" ", "");
                AgencyUser.Country = "Guinée";
                AgencyUser.Email = oa.Email.Trim().Replace(" ", "");
                AgencyUser.Login = oa.Login.Trim().Replace(" ", "");
                AgencyUser.Password = oa.Password.Trim().Replace(" ", "");
                AgencyUser.Profil = oa.Profil.Trim().Replace(" ", "");
                AgencyUser.IsActive = true;
                AgencyUser.IsFirstLogin = true;
                AgencyUser.CreatedDate = DateTime.UtcNow;
                AgencyUser.UpdatedDate = DateTime.UtcNow;
                await _CatalogDbContext.AgencyUsers.AddAsync(AgencyUser);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = AgencyUser;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.ToString();
                rp.Code = 400;

            }
            return rp;
        }

        public async Task<ResponseBody<AgencyUser>> updatePassword(ChangePasswordDto cp)
        {
            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {
                AgencyUser st = await _CatalogDbContext.AgencyUsers.FindAsync(cp.Id);
                if (st != null)
                {
                    if (st.Password == cp.OldPassword.Trim().Replace(" ", ""))
                    {
                        st.Password = cp.NewPassword.Trim().Replace(" ", "");
                        st.UpdatedDate = DateTime.UtcNow;
                        //agency.fkIdStaff = ag.fkIdStaff;
                        st.IsFirstLogin = false;
                        _CatalogDbContext.AgencyUsers.Update(st);
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
                    rp.Msg = "user not found!";
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

        public async Task<ResponseBody<AgencyUser>> activateOrDeactiveAgencyUser(ActiveBlockDto dto)
        {
            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {
                AgencyUser agu = await _CatalogDbContext.AgencyUsers.Where(a => a.IdAgencyUser == dto.IdUser).FirstOrDefaultAsync();
                if (agu != null)
                {

                    agu.IsActive = !agu.IsActive;
                    agu.FkIdStaff = dto.FkIdStaff;
                    agu.UpdatedDate = DateTime.UtcNow;
                    _CatalogDbContext.AgencyUsers.Update(agu);
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
        public async Task<ResponseBody<AgencyUser>> blockOrDeblockAgencyUser(ActiveBlockDto dto)
        {
            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {
                AgencyUser agu = await _CatalogDbContext.AgencyUsers.Where(a => a.IdAgencyUser == dto.IdUser).FirstOrDefaultAsync();
                if (agu != null)
                {

                    agu.IsBlocked = !agu.IsBlocked;
                    agu.FkIdStaff = dto.FkIdStaff;
                    agu.UpdatedDate = DateTime.UtcNow;
                    _CatalogDbContext.AgencyUsers.Update(agu);
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

        public async Task<ResponseBody<AgencyUser>> updateAgencyUser(BodyAgencyUserUpdateDto oa,Ulid idAgencyUser)
        {

            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {
                AgencyUser AgencyUser = await _CatalogDbContext.AgencyUsers.Where(x => x.IdAgencyUser == idAgencyUser).FirstOrDefaultAsync();
                if (AgencyUser == null)
                {
                    rp.Msg = "User not found!";
                    rp.IsError = true;
                    rp.Code = 400;
                    return rp;
                }

                AgencyUser.FkIdStaff = oa.FkidStaff;
                AgencyUser.FkIdAgency = oa.FkidAgency;
                AgencyUser.FirstName = oa.FirstName.Trim().Replace(" ", "");
                AgencyUser.MiddleName = oa.MiddleName.Trim().Replace(" ", "");
                AgencyUser.LastName = oa.LastName.Trim().Replace(" ", "");
                AgencyUser.Phone = oa.Phone.Trim().Replace(" ", "");
                AgencyUser.Address = oa.Address.Trim().Replace(" ", "");
                AgencyUser.Email = oa.Email.Trim().Replace(" ", "");
                AgencyUser.Password = oa.Password.Trim().Replace(" ", "");
                AgencyUser.Profil = oa.Profil.Trim().Replace(" ", "");
                AgencyUser.IsActive = true;
                AgencyUser.UpdatedDate = DateTime.UtcNow;
                _CatalogDbContext.AgencyUsers.Update(AgencyUser);
                await _CatalogDbContext.SaveChangesAsync();
                rp.Body = AgencyUser;

            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = ex.ToString();
                rp.Code = 400;

            }
            return rp;
        }

        public async Task<ResponseBody<List<AgencyUser>>> findAllAgentsUser(int page = 1, int limit = 10)
        {
            ResponseBody<List<AgencyUser>> rp = new ResponseBody<List<AgencyUser>>();
            try
            {
                int skip = (page - 1) * (int)limit;
                if (_CatalogDbContext.AgencyUsers != null)
                {
                    int pageCount = (int)Math.Ceiling((decimal)_CatalogDbContext.AgencyUsers.Count() / limit);
                    var ps = await _CatalogDbContext.AgencyUsers.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                    //string jjj = "kkkkk";
                    if (ps != null && ps.Count() > 0)
                    {
                        rp.Body = ps;
                        rp.CurrentPage = page;
                        rp.TotalPage = pageCount;

                    }
                    else
                    {
                        rp.Body = new List<AgencyUser>();
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


        public async Task<ResponseBody<AgencyUser>> findAgencyUsertById(Ulid Id)
        {
            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {

                AgencyUser owa = await _CatalogDbContext.AgencyUsers.FindAsync(Id);
                await _CatalogDbContext.SaveChangesAsync();
                if (owa != null)
                {
                    rp.Body = owa;
                    return rp;

                }
                else
                {
                    rp.IsError = true;
                    rp.Body = null;
                    rp.Msg = "User of Agency not found";
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
        public async Task<ResponseBody<AgencyUser>> login(LoginDto bd)
        {
            string mtoken = "";
            ResponseBody<AgencyUser> rp = new ResponseBody<AgencyUser>();
            try
            {
                AgencyUser user = await _CatalogDbContext.AgencyUsers.Where(s => s.Login == bd.username).FirstOrDefaultAsync();
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
                          new Claim("id",user.IdAgencyUser.ToString()),
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
                            _CatalogDbContext.AgencyUsers.Update(user);
                            await _CatalogDbContext.SaveChangesAsync();
                        }

                        _CatalogDbContext.AgencyUsers.Update(user);
                        await _CatalogDbContext.SaveChangesAsync();

                        //_ca.HttpContext.Response.Cookies.Append("token", mtoken);
                        rp.Body = user;

                    }
                    else
                    {
                        user.LoginCount = user.LoginCount + 1;
                        _CatalogDbContext.AgencyUsers.Update(user);
                        await _CatalogDbContext.SaveChangesAsync();
                        if (user.LoginCount > 5)
                        {
                            user.IsBlocked = true;
                            _CatalogDbContext.AgencyUsers.Update(user);
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
        public async Task<ResponseBody<List<AgencyUser>>> findAgencyUsertByAgency(Ulid IdAgency, int page = 1, int limit = 10)
        {
            ResponseBody<List<AgencyUser>> rp = new ResponseBody<List<AgencyUser>>();

            try
            {
                int skip = (page - 1) * (int)limit;
                var req = _CatalogDbContext.AgencyUsers.Where(a => a.FkIdAgency == IdAgency);
                int pageCount = (int)Math.Ceiling((decimal)req.Count() / limit);

                var ps = await req.OrderByDescending(c => c.CreatedDate).Skip(skip).Take(limit).ToListAsync();
                await _CatalogDbContext.SaveChangesAsync();
                if (ps != null && ps.Count() > 0)
                {
                    rp.Body = ps;
                    rp.CurrentPage = page;
                    rp.TotalPage = pageCount;

                }
                else
                {
                    rp.Body = new List<AgencyUser>();
                }


            }
            catch (Exception ex)
            {
                rp.IsError = true;
                rp.Msg = "error";
                rp.Code = 400;
            }
            return rp;
        }
    }


}

