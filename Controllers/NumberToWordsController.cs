using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NumberToWordsController : ControllerBase
{
    private static readonly string[] unidades = { "", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve" };
    private static readonly string[] decenas = { "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete", "dieciocho", "diecinueve" };
    private static readonly string[] decenas10 = { "", "", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa" };
    private static readonly string[] centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos" };

    /// <summary>
    /// Funcion convertir a letras que permite convertir numeros a letras entre el o y el 9,999,999,999
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    [HttpGet("{number}")]
    public IActionResult ConvertToWords(long number)
    {
        string words = "";
        try
        {
            if (number < 0 || number > 9999999999)
            {
                return BadRequest("El número debe estar entre 0 y 9,999,999,999.");
            }

            words = ConvertNumberToWords(number);
        }
        catch (System.Exception)
        {
            return BadRequest("El número debe estar entre 0 y 9,999,999,999.");
        }

        return Ok(new { number, words });
    }

    /// <summary>
    /// Permite convertir un numero bigInt o entero grande a letras incluyendo negativos
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private string ConvertNumberToWords(long number)
    {
        string result = "";
        bool negativo = false;
        if (number < 0)
        {
            number = number * (-1);
            negativo = true;
        }

        if (number == 0) return "cero";

        if ((number / 1000000000) > 0)
        {
            result += number >= 1000000000 && number < 2000000000 ? "un mil millones " : ConvertHundreds(number / 1000000000) + " mil millones ";
            number %= 1000000000;
        }

        if ((number / 1000000) > 0)
        {
            result += number >= 1000000 && number < 2000000 ? "un millon " : ConvertHundreds(number / 1000000) + " millones ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            result += number >= 1000 && number < 2000 ? "un mil " : ConvertHundreds(number / 1000) + " mil ";
            number %= 1000;
        }

        if (number > 0)
        {
            result += ConvertHundreds(number);
        }

        return (negativo ? " menos " : "") + result.Trim();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private string ConvertHundreds(long number)
    {
        string result = "";

        if (number == 100) return "cien";

        if ((number / 100) > 0)
        {
            result += centenas[number / 100] + " ";
            number %= 100;
        }

        if (number > 0)
        {
            if (number < 10)
                result += unidades[number];
            else if (number < 20)
                result += decenas[number - 10];
            else
            {
                result += decenas10[number / 10];
                if ((number % 10) > 0)
                    result += " y " + unidades[number % 10];
            }

            if (number < 30 & number > 20)
            {
                result = result.Replace("e y ", "i");
            }
        }

        return result.Trim();
    }
    
}