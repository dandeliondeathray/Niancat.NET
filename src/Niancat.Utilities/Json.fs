namespace Niancat.Utilities

module Json =

    open Newtonsoft.Json
    open Newtonsoft.Json.Serialization

    let private serializerSettings = JsonSerializerSettings(ContractResolver = CamelCasePropertyNamesContractResolver())

    let serialize v = JsonConvert.SerializeObject(v, serializerSettings)

    let deserialize<'a> json = JsonConvert.DeserializeObject<'a>(json, serializerSettings)
