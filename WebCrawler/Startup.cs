using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simple_web_crawler.Channel;
using SimpleWebCrawler.client.Http;
using SimpleWebCrawler.Factory;
using SimpleWebCrawler.Services.HtmlParser;
using SimpleWebCrawler.Services.Priter;
using SimpleWebCrawler.Services.QueueService;
using SimpleWebCrawler.Services.WebCrawlerProcessor;

namespace WebCrawler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IChannelBuffer, ChannelBuffer1>();
            services.AddSingleton<IHttpClient>(new HttpClientWrapper(new HttpClient()));
            
            //builder.Services.AddSingleton<IProducerService>(new ProducerService(3));
            services.AddSingleton<IProducerService, ProducerService>();
            services.AddSingleton<IHtmlParser, HtmlParser>();
            services.AddSingleton<IConsumerService, ConsumerService>();
            services.AddSingleton<IPrinter, Printer>();
            services.AddSingleton<IProcessor, Process>();
            services.AddSingleton<IChannelBuffer, ChannelBuffer1>();

            Channel<string[]> webCrawlerChannel = Channel.CreateBounded<string[]>(new BoundedChannelOptions(int.MaxValue)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,//TODO Could be changed to multiple writer to improve read performance
                SingleWriter = true
            });

            services.AddSingleton<IChannelFactory>(new ChannelFactory(webCrawlerChannel));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "WebCrawler Project", Version = "V1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Webcrawler Project");
            });
        }
    }
}
