﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace UrbanAirship
{
    /// <summary>
    /// Implements the iOS Platform operations.
    /// </summary>

    public class iOSPlatform : PlatfromBase
    {
        internal DataContractJsonSerializer payloadSerializer;
        internal iOSPlatform(Client client)
            : base(client)
        {
            payloadSerializer = new DataContractJsonSerializer(typeof(iOSRegistrationPayload));
        }


        protected override void OnRegisterDevice(string token)
        {
            using (HttpWebResponse response = this.Client.HttpPut("/device_tokens/" + token))
            {
                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created) // 201 or 200
                {
                    throw new UrbanAirshipException("Unable to register device token");
                }
            }
        }

        protected override void OnRegisterDeviceWithAlias(string token, string alias)
        {
            iOSRegistrationPayload registration = new iOSRegistrationPayload() { DeviceAlias = alias };
            using (HttpWebResponse response = this.Client.HttpPut("/device_tokens/" + token, registration, this.payloadSerializer))
            {
                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created) // 201 or 200
                {
                    throw new UrbanAirshipException("Unable to register device token");
                }
            }
        }

        [DataContract]
        public class iOSRegistrationPayload
        {
            /// <summary>
            /// Alias Property.
            /// </summary>
            [DataMember(Name = "alias", EmitDefaultValue = false)]
            public string DeviceAlias { get; set; }

            /// <summary>
            /// Tags Property, not implement on this library
            /// </summary>
            [DataMember(Name = "tags", EmitDefaultValue = false)]
            public string DeviceTags { get; set; }
        }
    }
}
