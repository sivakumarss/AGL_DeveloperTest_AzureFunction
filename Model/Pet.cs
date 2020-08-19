using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AGL_DeveloperTestFunc.Model
{
    public class Pet
    {
        [JsonPropertyName("name")]
        public string PetName { get; set; }

        [JsonPropertyName("type")]
        public string PetType { get; set; }
    }
}
