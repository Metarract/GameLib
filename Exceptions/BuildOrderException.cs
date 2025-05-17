namespace Metarract.Exceptions;
public class BuildOrderException()
  : System.Exception(
    "Builder ran in the wrong order. Ensure the client is unable to receive this error on their own, and check that you are running your methods in the correct order");
