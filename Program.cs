using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortgage_Calculator
{
    internal class Program
    {
        static void Main()
        {
            // ===== VARIABLES =====
            double housePrice = 0;
            double houseMarketValue = 0;
            double downPayment = 0;
            double interestRate = 0;
            double hoaFees = 0;
            double buyersMonthlyIncome = 0;
            double term = 0;

            double loanTotal = 0;
            double equity = 0;
            double loanInsurance = 0;
            double monthlyLoanPayment = 0;
            double monthlyHoaFees = 0;
            double propertyTaxesMonthly = 0;
            double homeInsuranceMonthly = 0;
            double totalMonthlyPayment = 0;

            // get input from user
            getLoanDetails(ref housePrice, ref houseMarketValue, ref downPayment, ref interestRate, ref hoaFees, ref buyersMonthlyIncome, ref term);

            // Method Calling
            loanTotal = calculateLoanTotal(housePrice, downPayment);
            Console.WriteLine("Loan total = " + loanTotal);
            equity = calculateEquity(loanTotal, houseMarketValue);

            // display equity percentage and value of home at inception
            Console.WriteLine($"Current equity: {equity}");
            Console.WriteLine($"Current house value: {houseMarketValue}");

            // Method Calling v2 aka doing stuff
            loanInsurance = calculateLoanInsurance(equity, loanTotal, houseMarketValue);
            monthlyLoanPayment = calculateMonthlyLoanPayment(loanTotal, interestRate, term);
            monthlyHoaFees = calculateMonthlyHoaFees(hoaFees);
            propertyTaxesMonthly = calculateMonthlyPropertyTaxes(houseMarketValue);
            homeInsuranceMonthly = calculateMonthlyHomeInsurance(houseMarketValue);
            totalMonthlyPayment = calculateTotalMonthlyPayment(monthlyLoanPayment, monthlyHoaFees, propertyTaxesMonthly, homeInsuranceMonthly, loanInsurance);
            Console.WriteLine("Monthly payment = " + totalMonthlyPayment);

            // display info

            // recommend approve or deny
            if (approvedForLoan(totalMonthlyPayment, buyersMonthlyIncome))
            {
                Console.WriteLine("Your loan application is approved, congratulations.");
            }
            else
            {
                Console.WriteLine("Your loan application is DENIED.");
                Console.WriteLine("Place more money down or look at buying a more affordable home.");
            }

            // keeps terminal displayed correctly
            Console.ReadLine();
        }

        static void getLoanDetails(ref double housePrice, ref double houseMarketValue, ref double downPayment, ref double interestRate, ref double hoaFees, ref double buyersMonthlyIncome, ref double term)
        {

            Console.WriteLine("Enter the house price.");
            getUserInput("Invalid house price.", ref housePrice);
            Console.WriteLine("Enter the house market value.");
            getUserInput("Invalid house market value.", ref houseMarketValue);
            Console.WriteLine("Enter the down payment.");
            getUserInput("Invalid down payment.", ref downPayment);
            Console.WriteLine("Enter the interest rate.");
            getUserInput("Invalid interest rate.", ref interestRate);
            Console.WriteLine("Enter the hoa fees.");
            getUserInput("Invalid hoa fees.", ref hoaFees);
            Console.WriteLine("Enter your monthly income.");
            getUserInput("Invalid monthly income.", ref buyersMonthlyIncome);
            Console.WriteLine("Enter your loan term (15 or 30).");
            getUserInput("Invalid loan term.", ref term);

            return;
        }

        static double calculateLoanTotal(double housePrice, double downPayment)
        {
            double initialLoanAmount = housePrice - downPayment;
            double originationFee = .01 * initialLoanAmount;
            const double CLOSINGCOSTS = 2500;

            return initialLoanAmount + originationFee + CLOSINGCOSTS;
        }

        static double calculateEquity(double loanTotal, double houseMarketValue)
        {
            return houseMarketValue - loanTotal;
        }

        static double calculateLoanInsurance(double equity, double loanTotal, double houseMarketValue)
        {

            if (equity < (houseMarketValue * .1))
            {
                return (loanTotal * .01) / 12;
            }

            return 0;
        }

        /*
         	Loan Payment = P * (r / n) * [(1 + r / n)^n(t)] / [(1 + r / n)^n(t) - 1]
	        P: Principle (loan amount)
	        r: Annual Interest Rate
	        n: Number of payments per year
	        t: Term (number of years for the loan)
         */
        static double calculateMonthlyLoanPayment(double P, double r, double t)
        {
            const double n = 12;
            return P * (r / n) * (Math.Pow((1 + r / n), n * t)) / (Math.Pow((1 + r / n), n * t - 1));
        }

        static double calculateMonthlyHoaFees(double hoaFees)
        {
            return hoaFees / 12;
        }

        static double calculateMonthlyPropertyTaxes(double houseMarketValue)
        {
            double PROPERTYTAX = .0125;
            return (PROPERTYTAX * houseMarketValue) / 12;
        }

        static double calculateMonthlyHomeInsurance(double houseMarketValue)
        {
            double INSURANCERATE = .0075;
            return (INSURANCERATE * houseMarketValue) / 12;
        }

        static double calculateTotalMonthlyPayment(double monthlyLoanPayment, double monthlyHoaFees, double propertyTaxesMonthly, double homeInsuranceMonthly, double loanInsurance)
        {
            return monthlyLoanPayment + monthlyHoaFees + propertyTaxesMonthly + homeInsuranceMonthly + loanInsurance;
        }

        static bool approvedForLoan(double totalMonthlyPayment, double buyersMonthlyIncome)
        {
            if (totalMonthlyPayment >= (buyersMonthlyIncome * .25))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static void getUserInput(string errorMessage, ref double variableToFill)
        {
            while (true)
            {
                try
                {
                    variableToFill = double.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.WriteLine($"{errorMessage}" + " Please try again.");
                }
            }
        }
    }
}
