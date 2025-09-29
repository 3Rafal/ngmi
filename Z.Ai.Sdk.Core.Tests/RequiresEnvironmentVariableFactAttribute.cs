public class RequiresEnvironmentVariableFactAttribute : FactAttribute
{
    public RequiresEnvironmentVariableFactAttribute(string variableName)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(variableName)))
        {
            Skip = $"Test skipped because {variableName} is not set.";
        }
    }
}