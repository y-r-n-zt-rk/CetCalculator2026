using System.Globalization;
namespace MyFirstApp;

public partial class MainPage : ContentPage
{
    private double firstNumber = 0;
    private string currentOperator = "";
    private bool isFirstDigit = true;
    private string operationHistory = "";
    
    public MainPage()
    {
        InitializeComponent();
    }

    private void DigitClicked(object? sender, EventArgs e)
    {
        Button? digit = sender as Button;
        if (isFirstDigit)
        {
            Screen.Text = (string)digit?.CommandParameter;
            isFirstDigit = false;
        }
        else
        { 
            Screen.Text += (string)digit?.CommandParameter; 
        }
    } 
    private void SumOperator_OnClicked(object? sender, EventArgs e)
    {
        Calculate("+");
    }

    private void MinusOperator_OnClicked(object? sender, EventArgs e)
    {
        Calculate("-");
    }

    private void MultiplyOperator_OnClicked(object? sender, EventArgs e)
    {
        Calculate("*");
    }

    private void DivisionOperator_OnClicked(object? sender, EventArgs e)
    {
        Calculate("/");
    }

    private void EqualOperator_OnClicked(object? sender, EventArgs e)
    {
        Calculate("=");
    }
    
    private bool TryCalculate(double first, double second, string op, out double result)
    {
        result = 0;

        switch (op)
        {
            case "+": result = first + second;
                break;

            case "-": result = first - second;
                break;

            case "*": result = first * second;
                break;

            case "/":
                if (second == 0)
                    return false;
                result = first / second;
                break;

            default: return false;
        }

        return true;
    }

    private string FormatNumber(double number)
    {
        return number.ToString("G", CultureInfo.InvariantCulture);
    }

    private string DisplayOperator(string op)
    {
        return op switch
        {
            "*" => "×",
            "/" => "÷",
            _ => op
        };
    }
    
    
    
    
    
    
    
    private void Calculate(string op)
    {
        if (op == "=")
        {
            if (currentOperator == "" || isFirstDigit)
                return;

            double secondNumber = double.Parse(
                Screen.Text, CultureInfo.InvariantCulture);

            if (!TryCalculate(firstNumber, secondNumber, currentOperator, out double result))
            {
                Screen.Text = "Cannot divide by zero";
                HistoryScreen.Text = "";
                currentOperator = "";
                operationHistory = "";
                isFirstDigit = true;
                return;
            }

            operationHistory += $" {FormatNumber(secondNumber)} =";
            HistoryScreen.Text = operationHistory;
            Screen.Text = FormatNumber(result);

            firstNumber = result;
            currentOperator = "";
            operationHistory = "";
            isFirstDigit = true;
            return;
        }

        double currentNumber = double.Parse(
            Screen.Text, CultureInfo.InvariantCulture);
        
        if (currentOperator == "")
        {
            firstNumber = currentNumber;
            currentOperator = op;
            operationHistory = $"{FormatNumber(firstNumber)} {DisplayOperator(op)}";
        }
        
        else if (isFirstDigit)
        {
            currentOperator = op;
            operationHistory = operationHistory.Substring(0, operationHistory.Length - 1) + DisplayOperator(op);
        }
        
        else 
        { 
            if (!TryCalculate(firstNumber, currentNumber, currentOperator, out double result)) 
            {
                Screen.Text = "Cannot divide by zero";
                HistoryScreen.Text = "";
                currentOperator = "";
                operationHistory = "";
                isFirstDigit = true;
                return;
            }

            operationHistory += $" {FormatNumber(currentNumber)} {DisplayOperator(op)}";
            firstNumber = result;
            Screen.Text = FormatNumber(result);
            currentOperator = op;
        }

        HistoryScreen.Text = operationHistory;
        isFirstDigit = true;
    }
    
    private void Percentage_OnClicked(object? sender, EventArgs e)
    {
        if (!double.TryParse(Screen.Text, NumberStyles.Float,
                CultureInfo.InvariantCulture, out double currentNumber))
            return;

        double percentage;

        if (currentOperator == "+" || currentOperator == "-")
        {
            percentage = firstNumber * currentNumber / 100;
        }
        else
        {
            percentage = currentNumber / 100;
        }

        Screen.Text = FormatNumber(percentage);
        isFirstDigit = false;
    }
    
    private void Backspace_OnClicked(object? sender, EventArgs e)
    {
        string currentText = Screen.Text ?? "0";

        if (currentText.Length <= 1)
        {
            Screen.Text = "0";
            isFirstDigit = true; 
        }
        else
        {
            Screen.Text = currentText.Substring(0, currentText.Length - 1);
            isFirstDigit = false;
        }
    }
    
    private void Clear_OnClicked(object? sender, EventArgs e) 
    {
        Screen.Text = "0";
        HistoryScreen.Text = "";
        firstNumber = 0;
        currentOperator = "";
        isFirstDigit = true;
    }
    
    private void Decimal_OnClicked(object? sender, EventArgs e)
    {
        if (isFirstDigit)
        {
            Screen.Text = "0.";
            isFirstDigit = false;
        }
        else if (!Screen.Text.Contains("."))
        {
            Screen.Text += ".";
        }
    }
}