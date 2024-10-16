using Lathiecoco.models;
using Lathiecoco.repository;
using Lathiecoco.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opt => opt.AddPolicy("CorsPolicy", c =>
{
    c.WithOrigins("http://localhost:3000/",
                                  "http://localhost:3000",
                                  "https://main.d30cpdwxrbf5uc.amplifyapp.com"
                                  )
       .AllowAnyHeader()
       .AllowAnyMethod();
}));


builder.Services.AddDbContext<CatalogDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("apic")));

builder.Services.AddControllers();


builder.Services.AddScoped<BilllerInvoiceRep, BillerInvoiceService>();
builder.Services.AddScoped<AccountingRep, AccountingService>();
builder.Services.AddScoped<CustomerWalletRep, CustomerWalletService>();
builder.Services.AddScoped<InvoiceWalletRep, InvoiceWalletService>();
builder.Services.AddScoped<FeesSendRep, FeeSendService>();

builder.Services.AddScoped<AccountingOpWalletRep, AccountingOpWalletServ>();
builder.Services.AddScoped<InvoiceAgentRep, InvoiceAgentCustomerServ>();

builder.Services.AddScoped<PaymentModeRep, PaymentModeServ>();
builder.Services.AddScoped<AccountingPrincipalRep, AccountingPrincipalServ>();
builder.Services.AddScoped<AgentOwnerRep, AgentOwnerServ>();

builder.Services.AddScoped<InvoiceStartupMasterRep, InvoiceStartupMasterServ>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(
    x => x.Cookie.Name = "token"
)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["token"];
                return Task.CompletedTask;
            }
        };
    }
    );


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
