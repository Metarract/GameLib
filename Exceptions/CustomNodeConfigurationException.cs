namespace Metarract.Exceptions;

public class CustomNodeConfigurationException(string[] warning)
  : Exception($"Custom Node configuration error(s): {string.Join(Environment.NewLine, warning)}");
