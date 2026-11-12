using Newtonsoft.Json;
using System;

namespace SisPo.Pages.Input.POImexInv
{
  public class Signing
  {
    public string Subject;
    [JsonProperty("SigningTime")]
    public DateTimeOffset SigningTime;
  }
}
