using FluentValidation.AspNetCore;
using graduationProject.Api.Authentication;
using graduationProject.Api.Persistence;
using graduationProject.Api.Seeds;
using graduationProject.Api.Services;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace graduationProject.Api
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			// Add services to the container.
			services.AddControllers();

			//Add Cors
			//var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
			//services.AddCors(options =>
			//	options.AddDefaultPolicy(builder => builder
			//								.AllowAnyMethod()
			//								.AllowAnyHeader()
			//								.WithOrigins(allowedOrigins!)
			//	)
			//);
			services.AddCors(corsOptions =>
			{
				corsOptions.AddPolicy("Default", PolicyBuilder =>
				{
					PolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
				});

			});

			services
				.AddSwaggerConfig()
				.AddDbContextConfig(configuration)
				.AddMapsterConfig()
				.AddFluentValidationConfig()
				.AddAuthConfig(configuration);

			//add my services
			services.AddScoped<IJobService, JobService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IImageService, ImageService>();
			services.AddScoped<ICategoryService,CategoryService>();
			services.AddScoped<IProjectService, ProjectService>();
			services.AddScoped<IOfferService, OfferService>();
			services.AddScoped<IContactUsService, ContactUsService>();


			return services;
		}

		private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			return services;
		}
		private static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
		{
			//Add Dbcontext
			var connectionString = configuration.GetConnectionString("DefaultConnection") ??
			throw new InvalidOperationException("connectionString 'DefaultConnection' not found");

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
			return services;
		}
		private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
		{
			//Add Mapster
			var mappingConfig = TypeAdapterConfig.GlobalSettings;
			mappingConfig.Scan(Assembly.GetExecutingAssembly());
			services.AddSingleton<IMapper>(new Mapper(mappingConfig));
			return services;
		}
		private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
		{
			// Add Fluent Validation
			services
				.AddFluentValidationAutoValidation()
				.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			return services;
		}
		private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
		{
			//single instance throuhout the project
			services.AddSingleton<IJwtProvider, JwtProvider>();

			// add identity UserManager and IdentityRole
			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddSignInManager()
				.AddRoles<IdentityRole>();

			// add IOptions
			//services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
			services.AddOptions<JwtOptions>()
				.BindConfiguration(JwtOptions.SectionName)
				.ValidateDataAnnotations()
				.ValidateOnStart();

			var jwtSetting = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
			services
				.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
				{
					options.SaveToken = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting?.Key!)),
						ValidIssuer = jwtSetting?.Issuer,
						ValidAudience = jwtSetting?.Audience
					};
				});

			return services;
		}
	}
}
