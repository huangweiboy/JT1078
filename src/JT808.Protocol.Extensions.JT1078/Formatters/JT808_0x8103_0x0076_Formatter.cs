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
    public class JT808_0x8103_0x0076_Formatter : IJT808MessagePackFormatter<JT808_0x8103_0x0076>
    {
        public JT808_0x8103_0x0076 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x8103_0x0076 jT808_0X8103_0X0076 = new JT808_0x8103_0x0076();
            jT808_0X8103_0X0076.ParamId = reader.ReadUInt32();
            jT808_0X8103_0X0076.ParamLength = reader.ReadByte();
            jT808_0X8103_0X0076.AVChannelTotal = reader.ReadByte();
            jT808_0X8103_0X0076.AudioChannelTotal = reader.ReadByte();
            jT808_0X8103_0X0076.VudioChannelTotal = reader.ReadByte();
            var channelTotal = jT808_0X8103_0X0076.AVChannelTotal + jT808_0X8103_0X0076.AudioChannelTotal + jT808_0X8103_0X0076.VudioChannelTotal;//通道总数
            if (channelTotal > 0) {
                jT808_0X8103_0X0076.AVChannelRefTables = new List<JT808_0x8103_0x0076_AVChannelRefTable>();
                var formatter = config.GetMessagePackFormatter<JT808_0x8103_0x0076_AVChannelRefTable>();
                for (int i = 0; i < channelTotal; i++)
                {               
                    jT808_0X8103_0X0076.AVChannelRefTables.Add(formatter.Deserialize(ref reader, config));
                }
            }
            return jT808_0X8103_0X0076;
        }

        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x8103_0x0076 value, IJT808Config config)
        {
            writer.WriteUInt32(value.ParamId);
            writer.Skip(1,out int position);
            writer.WriteByte(value.AVChannelTotal);
            writer.WriteByte(value.AudioChannelTotal);
            writer.WriteByte(value.VudioChannelTotal);
            if (value.AVChannelRefTables.Any()) {
                var formatter = config.GetMessagePackFormatter<JT808_0x8103_0x0076_AVChannelRefTable>();
                foreach (var AVChannelRefTable in value.AVChannelRefTables)
                {
                    formatter.Serialize(ref writer, AVChannelRefTable, config);
                }
            }
            writer.WriteByteReturn((byte)(writer.GetCurrentPosition()- position-1), position);
        }
    }
}
