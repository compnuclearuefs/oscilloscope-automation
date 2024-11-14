using System;
using System.Collections.Generic;
using System.Text;

namespace comunicacaoOciloscopio.classes
{
    class Tools
    {
        // Função para converter uma data do calendário gregoriano para data juliana com frações de dias
        public static double ConvertDateToJulian(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            // Ajusta o ano e mês para o calendário juliano
            if (month <= 2)
            {
                year--;
                month += 12;
            }

            // Calcula o valor de A e B, que são usados para ajustar o cálculo para o calendário gregoriano
            int A = year / 100;
            int B = 2 - A + (A / 4);

            // Calcula a data juliana sem incluir a fração de dia
            double julianDay = Math.Floor(365.25 * (year + 4716))
                             + Math.Floor(30.6001 * (month + 1))
                             + day + B - 1524.5;

            // Adiciona a fração do dia (hora, minuto, segundo)
            double fractionOfDay = (date.Hour + (date.Minute / 60.0) + (date.Second / 3600.0)) / 24.0;

            return julianDay + fractionOfDay;
        }

        public static DateTime ConvertJulianToGregorian(double julianDate)
        {
            // Data inicial do calendário juliano, correspondente ao Unix epoch em dias julianos
            const double julianEpoch = 2440587.5; // Data juliana para 1970-01-01 00:00:00 UTC

            // Calcula o número de segundos desde o Unix epoch
            double secondsSinceEpoch = (julianDate - julianEpoch) * 86400;

            // Converte o valor em segundos para uma data do tipo DateTime (UTC)
            DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            date = date.AddSeconds(secondsSinceEpoch);

            return date;
        }

        public static DateTime ConvertUtcToTimeZone(DateTime utcDate, string timeZoneId)
        {
            // Obtém o fuso horário desejado a partir do identificador
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // Converte o horário UTC para o horário do fuso especificado
            DateTime convertedDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, timeZone);

            return convertedDate;
        }

        public static double applyJulianOffset(double julianDate)
        {
            double julianOffset = 2451544.5;
            return julianDate - julianOffset;
        }

        public static double correctJulianOffset(double julianDate)
        {
            double julianOffset = 2451544.5;
            return julianDate + julianOffset;
        }

    }
}
