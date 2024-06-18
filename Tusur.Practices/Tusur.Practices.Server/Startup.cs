using Microsoft.EntityFrameworkCore;
using Tusur.Practices.Persistence.Database;
using Tusur.Practices.Application.UseCases;
using Tusur.Practices.Persistence.Services;
using Tusur.Practices.Jwt.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Tusur.Practices.Persistence.Database.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Tusur.Practices.Persistence.UnitsOfWork;
using Tusur.Practices.Persistence.Repositories;
using Tusur.Practices.Persistence.Repositories.Implementations;
using Tusur.Practices.Application.Ports.Input;
using Tusur.Practices.Application.Ports.Output;
using Tusur.Practices.Application.Domain.Entities;

namespace Tusur.Practices.Server
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(_configuration.GetConnectionString("Default"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.WithOrigins("http://localhost", "http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            Register(services);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Jwt.Defaults.JwtBearerDefaults.Issuer,
                        ValidateAudience = true,
                        ValidAudience = Jwt.Defaults.JwtBearerDefaults.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwt.Defaults.JwtBearerDefaults.TokenSecret)),
                        ValidateIssuerSigningKey = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder
                        (JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter a valid token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("Default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void Register(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services & Managers

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenService, TokenService>();

            services.AddTransient<IService<ApplicationEntity>, Service<ApplicationEntity, Persistence.Database.Entities.Application>>();
            services.AddTransient<IService<ApprovedPracticeEntity>, Service<ApprovedPracticeEntity, ApprovedPractice>>();
            services.AddTransient<IService<ApprovedStudyPlanEntity>, Service<ApprovedStudyPlanEntity, ApprovedStudyPlan>>();
            services.AddTransient<IService<CommentEntity>, Service<CommentEntity, Comment>>();
            services.AddTransient<IService<ContractEntity>, Service<ContractEntity, Contract>>();
            services.AddTransient<IService<ContractContentEntity>, Service<ContractContentEntity, ContractContent>>();
            services.AddTransient<IService<DegreeEntity>, Service<DegreeEntity, Degree>>();
            services.AddTransient<IService<DepartmentEntity>, Service<DepartmentEntity, Department>>();
            services.AddTransient<IService<DepartmentHeadEntity>, Service<DepartmentHeadEntity, DepartmentHead>>();
            services.AddTransient<IService<FacultyEntity>, Service<FacultyEntity, Faculty>>();
            services.AddTransient<IService<GroupEntity>, Service<GroupEntity, Group>>();
            services.AddTransient<IService<NotificationEntity>, Service<NotificationEntity, Notification>>();
            services.AddTransient<IService<OrganizationEntity>, Service<OrganizationEntity, Organization>>();
            services.AddTransient<IService<PracticeEntity>, Service<PracticeEntity, Practice>>();
            services.AddTransient<IService<PracticeDateEntity>, Service<PracticeDateEntity, PracticeDate>>();
            services.AddTransient<IService<PracticeKindEntity>, Service<PracticeKindEntity, PracticeKind>>();
            services.AddTransient<IService<PracticeProfileEntity>, Service<PracticeProfileEntity, PracticeProfile>>();
            services.AddTransient<IService<PracticeTypeEntity>, Service<PracticeTypeEntity, PracticeType>>();
            services.AddTransient<IService<ProxyEntity>, Service<ProxyEntity, Proxy>>();
            services.AddTransient<IService<RefreshTokenEntity>, Service<RefreshTokenEntity, RefreshToken>>();
            services.AddTransient<IService<SignatoryEntity>, Service<SignatoryEntity, Signatory>>();
            services.AddTransient<IService<SpecialtyEntity>, Service<SpecialtyEntity, Specialty>>();
            services.AddTransient<IService<StudentEntity>, Service<StudentEntity, Student>>();
            services.AddTransient<IService<StudyFieldEntity>, Service<StudyFieldEntity, StudyField>>();
            services.AddTransient<IService<StudyFormEntity>, Service<StudyFormEntity, StudyForm>>();
            services.AddTransient<IService<StudyPlanEntity>, Service<StudyPlanEntity, StudyPlan>>();
            services.AddTransient<IService<SupervisorEntity>, Service<SupervisorEntity, Supervisor>>();
            services.AddTransient<IService<TeacherEntity>, Service<TeacherEntity, Teacher>>();

            services.AddTransient<IApplicationManager, ApplicationManager>();
            services.AddTransient<IApproveManager, ApproveManager>();
            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IContractManager, ContractManager>();
            services.AddTransient<IFacultyManager, FacultyManager>();
            services.AddTransient<IGroupManager, GroupManager>();
            services.AddTransient<IOrganizationManager, OrganizationManager>();
            services.AddTransient<IParticipantManager, ParticipantManager>();
            services.AddTransient<IPracticeManager, PracticeManager>();
            services.AddTransient<IPropertyManager, PropertyManager>();
            services.AddTransient<IProxyManager, ProxyManager>();
            services.AddTransient<IStudyFieldManager, StudyFieldManager>();
            services.AddTransient<IStudyPlanManager, StudyPlanManager>();
            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<IUserAccountManager, UserAccountManager>();
            services.AddTransient<IUserPracticeManager, UserPracticeManager>();
        }
    }
}
