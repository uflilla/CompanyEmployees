using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Formatter.OutputFormatter
{
  public class CsvOutputFormatter : TextOutputFormatter
  {
    public CsvOutputFormatter()
    {
      SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
      SupportedEncodings.Add(Encoding.UTF8);
      SupportedEncodings.Add(Encoding.Unicode);
    }
    protected override bool CanWriteType(Type? type)
    {
      if (typeof(CompanyDto).IsAssignableFrom(type) ||
          typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type))
      {
        return base.CanWriteType(type);
      }
      return false;
    }
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
      var response = context.HttpContext.Response;
      var stringBuilder = new StringBuilder();
      if (context.Object is IEnumerable<CompanyDto>)
      {
        foreach (var company in (IEnumerable<CompanyDto>)context.Object)
        {
          FormatCsv(stringBuilder, company);
        }
      }
      else
      {
        FormatCsv(stringBuilder, (CompanyDto)context.Object);
      }
      await response.WriteAsync(stringBuilder.ToString());
    }
    private static void FormatCsv(StringBuilder stringBuilder, CompanyDto company)
    {
      stringBuilder.AppendLine($"{company.Id}|{company.Name}|{company.FullAddress}");
    }

    
  }
}
