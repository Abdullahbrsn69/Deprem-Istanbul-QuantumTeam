using Qu.Data;
using Qu.Utility;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Qu.Api.Helpers.Claims;
using static Qu.;

namespace Qu.API
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(Startup)); ;
            _environment = environment ?? throw new ArgumentNullException(nameof(Startup)); ;
        }

        #region ConfigureServices

        // Bu metot servisleri yapılandırmak için çalıştırılır.
        public void ConfigureServices(IServiceCollection services)
        {
            dbDsnStore.Dev = _configuration["ConnectionStrings:connStr"];
            dbDsnStore.Logger = _configuration["ConnectionStrings:connStrLogger"];
            dbDsnStore.CommandTimeout = Convert.ToInt32(_configuration.GetSection("AppSettings:Secret").Value);

            ControllerAndScopeServices(services);

            SwaggerAndCorsConfigurations(services);
        }

        #endregion

        #region SwaggerAndCorsConfigurations

        private void SwaggerAndCorsConfigurations(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Base64-encoded credentials."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // CORS politikasını tüm ortamlar için etkinleştir
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:3000") // React uygulamanızın çalıştığı adres
                                       .AllowAnyMethod()
                                       .AllowAnyHeader());
            });
        }

        #endregion

        #region ControllerAndScopeServices

        private static void ControllerAndScopeServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddControllersWithViews().AddNewtonsoftJson(); ;
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.All;
            });

            // Lookup
            services.AddScoped<ILookupService, LookupService>();
            services.AddScoped<ILookupDetailService, LookupDetailService>();

            // Logger
            services.AddScoped<IAppLogger, AppLogger>();

            // Claims ( Caching )
            services.AddScoped<IUserClaim, UserClaim>();
            services.AddHttpContextAccessor();
        }

        #endregion

        #region Configure

        // Bu metot HTTP istek pipeline'ını yapılandırmak için çalıştırılır.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAppLogger logger)
        {
            //if (env.IsDevelopment() == true)
            //{
            // Frontend eş zamanlı çalıştırma ayarı
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = ""; // Swagger UI'ı kök dizinde hizmet vermesini sağlar
            });
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    app.UseHsts(); // Sadece güvenlikli (sertifikalı) yerlerden istekleri kabul eder
            //}

            app.UseForwardedHeaders();
            app.UseHttpsRedirection(); // Sadece https isteklerini kabul eder
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            // Kimlik doğrulama ve yetkilendirme
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            DefineParameters(logger);
        }

        #endregion

        #region DefineParameters & Initialize Logger

        public async Task DefineParameters(IAppLogger logger)
        {
            while (!await dbRefs.RefreshSiteParameters())
            {
                logger.Log(LogCategory.Debug_1, nameof(Startup), nameof(DefineParameters), Events.Database, (int)EventType.Database.List, Message.Parameter, (int)MessageType.Parameter.DefineFailed);
                Thread.Sleep(100);
            }

            logger.Log(LogCategory.Trace_0, nameof(Startup), nameof(DefineParameters), Events.Database, (int)EventType.Database.List, Message.Parameter, (int)MessageType.Parameter.DefineSuccess);
        }

        #endregion
    }
}