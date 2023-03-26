namespace DriverRater.Tests.Plumbing;

using Newtonsoft.Json;

public static class  Approvals
{
    public static void VerifyAsJson(this object input)
    {
        var json = JsonConvert.SerializeObject(input);
        ApprovalTests.Approvals.VerifyJson(json);
    }
}