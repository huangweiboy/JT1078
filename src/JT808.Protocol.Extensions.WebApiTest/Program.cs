﻿using JT808.DotNetty.Abstractions.Dtos;
using JT808.DotNetty.WebApiClientTool;
using JT808.Protocol.Extensions.JT1078;
using JT808.Protocol.Extensions.JT1078.MessageBody;
using JT808.Protocol.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using WebApiClient.Extensions.DependencyInjection;

namespace JT808.Protocol.Extensions.WebApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection serviceDescriptors = new ServiceCollection();

            serviceDescriptors
                            .AddJT808Configure()
                            .AddJT1078Configure();

            serviceDescriptors.AddHttpApi<IJT808DotNettyWebApi>().ConfigureHttpApiConfig((c, p) =>
            {
                c.HttpHost = new Uri("http://localhost:12828/jt808api/");
                c.FormatOptions.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
                c.LoggerFactory = p.GetRequiredService<ILoggerFactory>();
            });

            IServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
            
            IJT808Config config = serviceProvider.GetRequiredService<IJT808Config>();
            JT808Serializer JT808Serializer = config.GetSerializer();

            string terminalPhoneNo = "";

            JT808Package jT808Package = new JT808Package();
            JT808Header header = new JT808Header();
            header.MsgId = 0x9101;
            header.MsgNum = 1;
            header.TerminalPhoneNo = terminalPhoneNo;
            jT808Package.Header = header;
            JT808_0x9101 jT808_0X9101 = new JT808_0x9101();
            jT808_0X9101.ServerIPAddress = "127.0.0.1";
            jT808_0X9101.ServerVideoChannelTcpPort = 1888;
            jT808_0X9101.ServerVideoChannelUdpPort = 0;
            jT808_0X9101.LogicalChannelNo = 1;
            jT808_0X9101.DataType = 1;
            jT808_0X9101.StreamType = 1;
            jT808Package.Bodies = jT808_0X9101;

            var data = JT808Serializer.Serialize(jT808Package);
            Console.WriteLine(JsonConvert.SerializeObject(data.ToHexString()));

            IJT808DotNettyWebApi JT808DotNettyWebApiClient = serviceProvider.GetRequiredService<IJT808DotNettyWebApi>();
            var result = JT808DotNettyWebApiClient.UnificationTcpSend(new JT808UnificationSendRequestDto
            {
                 TerminalPhoneNo= terminalPhoneNo,
                 Data= data
            }).GetAwaiter().GetResult();

            Console.WriteLine(JsonConvert.SerializeObject(result));

            //JT808Package jT808Package1 = new JT808Package();
            //JT808Header header1 = new JT808Header();
            //header1.MsgId = 0x9102;
            //header1.MsgNum = 2;
            //header1.TerminalPhoneNo = terminalPhoneNo;
            //jT808Package1.Header = header;
            //JT808_0x9102 jT808_0X9102 = new JT808_0x9102();
            //jT808_0X9102.LogicalChannelNo = 1;
            //jT808_0X9102.ControlCmd = 1;
            //jT808_0X9102.CloseAVData = 0;
            //jT808_0X9102.SwitchStreamType = 0;
            //jT808Package1.Bodies = jT808_0X9102;
            //var data1 = JT808Serializer.Serialize(jT808Package1);
            //Console.WriteLine(JsonConvert.SerializeObject(data1.ToHexString()));

            //var result1 = JT808DotNettyWebApiClient.UnificationTcpSend(new JT808UnificationSendRequestDto
            //{
            //    TerminalPhoneNo = terminalPhoneNo,
            //    Data = data1
            //}).GetAwaiter().GetResult();

            //Console.WriteLine(JsonConvert.SerializeObject(result1));

            Console.ReadKey();
        }
    }
}
