using Newtonsoft.Json;
using System;

namespace SisPo.Pages.Input.POImexInv
{
  public class Invoice
  {
    public string nbmst;
    public string khmshdon;
    public string khhdon;
    public string shdon;
    public string mhdon;
    public string dvtte;
    public string hdon;
    public string hsgcma;
    public string hsgoc;
    public string hthdon;
    public TaxRate[] thttltsuat;
    public Other[] ttkhac;
    public string thtttoan;
    public string id;
    public string nbdchi;
    public string nbten;
    public string nmdchi;
    public string nmmst;
    public string nmten;
    [JsonProperty("tdlap")]
    public DateTimeOffset tdlap;
    [JsonProperty("ncnhat")]
    public DateTimeOffset ncnhat;
    public string tgia;
    public string tgtcthue;
    public string tgtthue;
    public string tgtttbso;
    public string tgtttbchu;
    public string thdon;
    public string tthai;
    public string ttxly;
    public string mkhang;
    public string nbcks;
  }
}
