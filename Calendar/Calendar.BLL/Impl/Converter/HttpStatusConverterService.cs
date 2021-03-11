using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Calendar.BLL.Abstract.Converter;
using Calendar.Models.Attributes;
using Calendar.Models.Result;

namespace Calendar.BLL.Impl.Converter
{
    public class HttpStatusConverterService : IConverterService<int, ResponseMessageType>
    {
        private readonly Dictionary<ResponseMessageType, int> _msgToCode =
            new Dictionary<ResponseMessageType, int>();

        public HttpStatusConverterService()
        {
            foreach (ResponseMessageType messageType in Enum.GetValues(typeof(ResponseMessageType)))
            {
                MemberInfo memberInfo = messageType.GetType()
                    .GetMember(messageType.ToString()).FirstOrDefault();

                if (memberInfo != null)
                {
                    HttpStatusAttribute attribute = memberInfo.GetCustomAttribute<HttpStatusAttribute>();

                    if (attribute == null)
                        throw new MissingFieldException($"HttpStatusAttribute is absent in {messageType}");
                    _msgToCode.Add(messageType, attribute.Value);
                }

                else
                    throw new TargetInvocationException(
                        new TargetException($"Retrieving meta-data of {typeof(HttpStatusAttribute)} crashed"));
            }
        }

        public int Convert(ResponseMessageType messageType)
        {
            return _msgToCode[messageType];
        }
    }
}