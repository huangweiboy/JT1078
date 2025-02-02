﻿using JT808.Protocol.Extensions.JT1078.MessageBody;
using JT808.Protocol.Formatters;
using JT808.Protocol.Interfaces;
using JT808.Protocol.MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JT808.Protocol.Extensions.JT1078.Formatters
{
    public class JT808_0x1205_Formatter : IJT808MessagePackFormatter<JT808_0x1205>
    {
        public JT808_0x1205 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x1205 jT808_0x1205 = new JT808_0x1205();
            jT808_0x1205.MsgNum = reader.ReadUInt16();
            jT808_0x1205.AVResouceTotal = reader.ReadUInt32();
            var channelTotal = jT808_0x1205.AVResouceTotal;//音视频资源总数
            if (channelTotal > 0)
            {
                jT808_0x1205.AVResouces =new List<JT808_0x1205_AVResouce>();
                var formatter = config.GetMessagePackFormatter<JT808_0x1205_AVResouce>();
                for (int i = 0; i < channelTotal; i++)
                {
                    jT808_0x1205.AVResouces.Add(formatter.Deserialize(ref reader, config));
                }
            }
            return jT808_0x1205;
        }

        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x1205 value, IJT808Config config)
        {
            writer.WriteUInt16(value.MsgNum);
            writer.WriteUInt32(value.AVResouceTotal);
            if (value.AVResouces.Any())
            {
                var formatter = config.GetMessagePackFormatter<JT808_0x1205_AVResouce>();
                foreach (var AVResouce in value.AVResouces)
                {
                    formatter.Serialize(ref writer, AVResouce, config);
                }
            }
        }
    }
}