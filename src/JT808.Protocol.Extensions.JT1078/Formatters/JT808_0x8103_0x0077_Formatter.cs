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
    public class JT808_0x8103_0x0077_Formatter : IJT808MessagePackFormatter<JT808_0x8103_0x0077>
    {
        public JT808_0x8103_0x0077 Deserialize(ref JT808MessagePackReader reader, IJT808Config config)
        {
            JT808_0x8103_0x0077 jT808_0X8103_0X0077 = new JT808_0x8103_0x0077();
            jT808_0X8103_0X0077.ParamId = reader.ReadUInt32();
            jT808_0X8103_0X0077.ParamLength = reader.ReadByte();
            jT808_0X8103_0X0077.NeedSetChannelTotal = reader.ReadByte();
            if (jT808_0X8103_0X0077.NeedSetChannelTotal > 0) {
                jT808_0X8103_0X0077.SignalChannels = new List<JT808_0x8103_0x0077_SignalChannel>();
                var formatter = config.GetMessagePackFormatter<JT808_0x8103_0x0077_SignalChannel>();
                for (int i = 0; i < jT808_0X8103_0X0077.NeedSetChannelTotal; i++)
                {
                    jT808_0X8103_0X0077.SignalChannels.Add(formatter.Deserialize(ref reader, config));
                }
            }
            return jT808_0X8103_0X0077;
        }

        public void Serialize(ref JT808MessagePackWriter writer, JT808_0x8103_0x0077 value, IJT808Config config)
        {
            writer.WriteUInt32(value.ParamId);
            writer.Skip(1,out var position); 
            writer.WriteByte(value.NeedSetChannelTotal);
            if (value.SignalChannels.Any()) {
                var formatter = config.GetMessagePackFormatter<JT808_0x8103_0x0077_SignalChannel>();
                foreach (var signalChannel in value.SignalChannels)
                {
                    formatter.Serialize(ref writer, signalChannel, config);
                }
            }
            writer.WriteByteReturn((byte)(writer.GetCurrentPosition() - position - 1), position);
        }
    }
}
