using System;

namespace AvaloniaTask1.Models;

public class RationalNumber
{
    public int Numerator { get; private set; }
    public int Denominator { get; private set; }

    public RationalNumber(int numerator, int denominator)
    {
        if (denominator == 0) 
            throw new ArgumentException("Denominator cannot be zero");
        
        Numerator = numerator;
        Denominator = denominator;
        Normalize();
    }

    private void Normalize()
    {
        int gcd = GCD(Math.Abs(Numerator), Math.Abs(Denominator));
        Numerator /= gcd;
        Denominator /= gcd;

        if (Denominator < 0)
        {
            Numerator *= -1;
            Denominator *= -1;
        }
    }

    private static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);

    public override string ToString() => $"{Numerator}/{Denominator}";

    // Операторы
    public static RationalNumber operator +(RationalNumber a, RationalNumber b)
    {
        return new RationalNumber(
            a.Numerator * b.Denominator + b.Numerator * a.Denominator,
            a.Denominator * b.Denominator
        );
    }

    public static RationalNumber operator -(RationalNumber a, RationalNumber b)
    {
        return new RationalNumber(
            a.Numerator * b.Denominator - b.Numerator * a.Denominator,
            a.Denominator * b.Denominator
        );
    }

    public static RationalNumber operator *(RationalNumber a, RationalNumber b)
    {
        return new RationalNumber(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
    }

    public static RationalNumber operator /(RationalNumber a, RationalNumber b)
    {
        if (b.Numerator == 0) throw new DivideByZeroException();
        return new RationalNumber(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    }
}