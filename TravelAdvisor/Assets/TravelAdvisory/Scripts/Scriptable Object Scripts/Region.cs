public class Region
{
    public string Province_State { get; set; }
    public string Country_Region { get; set; }
    public int Confirmed { get; set; }
    public int Deaths { get; set; }
    public int Recovered { get; set; }
    public int Active { get; set; }
    public string Combined_Key { get; set; }
    public double Incident_Rate { get; set; }
    public double Case_Fatality_Ratio { get; set; }
    public int StatusIndex { get; set; }
}
