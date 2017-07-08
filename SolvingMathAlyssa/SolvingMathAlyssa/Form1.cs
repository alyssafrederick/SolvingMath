using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolvingMathAlyssa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GAMErightPanel.Visible = false;
            GAMErightPanel.Location = new Point(239, 185);
            GAMEleftPanel.Visible = false;
            GAMEleftPanel.Location = new Point(12, 185);
            SOLVERrightPanel.Visible = false;
            SOLVERleftPanel.Visible = false;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////


        private void solverOptionButton_Click(object sender, EventArgs e)
        {
            SOLVERrightPanel.Visible = true;
            SOLVERleftPanel.Visible = true;
            SOLVERenterButton.Visible = true;
            GAMErightPanel.Visible = false;
            GAMEleftPanel.Visible = false;    
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void gameOptionButton_Click(object sender, EventArgs e)
        {
            GAMErightPanel.Visible = true;
            GAMEleftPanel.Visible = true;
            SOLVERrightPanel.Visible = false;
            SOLVERleftPanel.Visible = false;
        }


        //for SOLVER//////////////////////////////////////////////////////////////////////////////////////////////////

        private void SOLVERequationToSolve_DoubleClick(object sender, EventArgs e)
        {
            SOLVERequationToSolve.Text = "";
        }

        private void SOLVERenterButton_Click(object sender, EventArgs e)
        {
            //(3+4)*(3+6/(3*2))
            string equation = SOLVERequationToSolve.Text;
            equation = equation.Replace(" ", "");
            float answer = 0;
            int lparenthesescount = 0;
            int rparenthesescount = 0;

            for (int i = 0; i < equation.Length; i++)
            {
                if (equation[i] == '(')
                {
                    lparenthesescount++;
                }
                else if(equation[i] == ')')
                {
                    rparenthesescount++;
                    if (lparenthesescount < rparenthesescount)
                    {
                        lparenthesescount = int.MinValue;
                    }
                }
            }

            if (lparenthesescount != rparenthesescount)
            {
                errorMessageBox.Visible = true;
                errorMessageBox.BringToFront();
            }


            else
            {
                //Parentheses
                while (equation.Contains('('))
                {
                    int lastindexoflp = equation.LastIndexOf('(');
                    int firstindexofrp = equation.IndexOf(')', lastindexoflp);

                    string newequation = equation.Substring(lastindexoflp + 1, firstindexofrp - lastindexoflp - 1);

                    float equationanswer = equationSolver(newequation);

                    equation = equation.Replace(string.Format("({0})", newequation), equationanswer.ToString());
                }

                answer = equationSolver(equation);
            }
            

            

            SOLVERanswer.Text = string.Format("{0:0.00}", answer);
            SOLVERanswer.TextAlign = ContentAlignment.MiddleCenter;
        }

        private float equationSolver(string equation)
        {
            equation = equation.Replace(" ", "");

            char[] AS = new char[] { '+', '-' };
            bool containsAS = false;
            float answer = 0;
            char pow = '^';
            while (true)
            {
                bool isNum1Negative = false;
                bool isNum2Negative = false;
                if (equation.Contains(pow))
                {
                    int lastindexofpow = equation.LastIndexOf(pow);
                    for (int i = 0; i < AS.Length; i++)
                    {
                        if (equation.LastIndexOf(AS[i]) > lastindexofpow)
                        {
                            if (AS[i] == '-')
                            {
                                isNum2Negative = !isNum2Negative;
                            }
                            equation = equation.Remove(equation.LastIndexOf(AS[i]), 1);
                            i = -1;
                        }
                        else
                        {
                            if (equation.LastIndexOf(AS[i]) == -1)
                            {
                                continue;
                            }
                            if (AS[i] == '-')
                            {
                                isNum1Negative = !isNum1Negative;
                            }
                            equation = equation.Remove(equation.LastIndexOf(AS[i]), 1);
                            i = -1;
                        }
                    }
                    lastindexofpow = equation.LastIndexOf(pow);
                    int countdown = 1;
                    int countup = 1;
                    int number1 = int.Parse(equation[lastindexofpow - countdown].ToString());
                    int number2 = int.Parse(equation[lastindexofpow + countup].ToString());
                    countup++;
                    countdown++;
                    int digit = 0;
                    while (lastindexofpow - countdown >= 0 && int.TryParse(equation[lastindexofpow - countdown].ToString(), out digit))
                    {
                        digit *= power(10, (countdown - 1));
                        number1 += digit;
                        countdown++;
                    }
                    if (isNum1Negative)
                    {
                        number1 *= -1;
                    }
                    while (lastindexofpow + countup < equation.Length && int.TryParse(equation[lastindexofpow + countup].ToString(), out digit))
                    {
                        number2 *= 10;
                        number2 += digit;
                        countup++;
                    }
                    if (isNum2Negative)
                    {
                        number2 *= -1;
                    }
                    string newequation = equation.Substring(lastindexofpow - countdown + 1, countup + countdown - 1);
                    equation = equation.Replace(newequation, power(number1, number2).ToString());
                    return float.Parse(equation);
                }
                else
                {
                    break;
                }
            }

            //MULTIPLY AND DIVIDE////////////////////////////////////////////////////////////////////////////////////

            char[] md = new char[]{'x', 'X', '*', '/', '÷'};
            bool containsMD = false;
            while (true)
            {
                bool isNum1Negative = false;
                bool isNum2Negative = false;
                for (int i = 0; i < md.Length; i++)
                {
                    if (equation.Contains(md[i]))
                    {
                        containsMD = true;
                        break;
                    }
                    containsMD = false;
                }
                if (containsMD)
                {
                    int closestOperator = int.MaxValue;
                    for (int i = 0; i < md.Length; i++)
                    {
                        if (equation.IndexOf(md[i]) < closestOperator && equation.IndexOf(md[i]) != -1)
                        {
                            closestOperator = equation.IndexOf(md[i]);
                        }
                    }
                    for (int i = 0; i < AS.Length; i++)
                    {
                        if (equation.LastIndexOf(AS[i]) > closestOperator)
                        {
                            if (AS[i] == '-')
                            {
                                isNum2Negative = !isNum2Negative;
                            }
                            equation = equation.Remove(equation.LastIndexOf(AS[i]), 1);
                            i = -1;
                        }
                        else
                        {
                            if (equation.LastIndexOf(AS[i]) == -1)
                            {
                                continue;
                            }
                            if (AS[i] == '-')
                            {
                                isNum1Negative = !isNum1Negative;
                            }
                            equation = equation.Remove(equation.LastIndexOf(AS[i]), 1);
                            i = -1;
                        }
                    }
                    for (int i = 0; i < md.Length; i++)
                    {
                        if (equation.IndexOf(md[i]) < closestOperator && equation.IndexOf(md[i]) != -1)
                        {
                            closestOperator = equation.IndexOf(md[i]);
                        }
                    }
                    int countdown = 1;
                    int countup = 1;
                    int number1 = int.Parse(equation[closestOperator - countdown].ToString());
                    int number2 = int.Parse(equation[closestOperator + countup].ToString());
                    countup++;
                    countdown++;
                    int digit = 0;
                    while (closestOperator - countdown >= 0 && int.TryParse(equation[closestOperator - countdown].ToString(), out digit))
                    {
                        digit *= power(10, (countdown - 1));
                        number1 += digit;
                        countdown++;
                    }
                    if (isNum1Negative)
                    {
                        number1 *= -1;
                    }
                    while (closestOperator + countup < equation.Length && int.TryParse(equation[closestOperator + countup].ToString(), out digit))
                    {
                        number2 *= 10;
                        number2 += digit;
                        countup++;
                    }
                    if (isNum2Negative)
                    {
                        number2 *= -1;
                    }
                    string newequation = equation.Substring(closestOperator - countdown + 1, countup + countdown - 1);
                    if(newequation.Contains('/') || newequation.Contains('÷'))
                    {
                        if(number2 == 0)
                        {
                            answer = 0;
                        }
                        else
                        {
                            answer = number1/number2;
                        }
                    }
                    else
                    {
                        answer = number1 * number2;
                    }
                    equation = equation.Replace(newequation, answer.ToString());
                    return float.Parse(equation);
                }
                else
                {
                    break;
                }
            }



            //ADDITION AND SUBTRACTION///////////////////////////////////////////////////////////////////////////////////////////////

            while (true)
            {
                containsAS = false;
                bool isNum1Negative = false;
                bool isNum2Negative = false;
                for (int i = 0; i < AS.Length; i++)
                {
                    if (equation.Contains(AS[i]))
                    {
                        containsAS = true;
                        break;
                    }
                    containsAS = false;
                }
                if (containsAS)
                {
                    int closestOperator = int.MaxValue;
                    for (int i = 0; i < AS.Length; i++)
                    {
                        if (equation.IndexOf(AS[i]) < closestOperator && equation.IndexOf(AS[i]) != -1)
                        {
                            closestOperator = equation.IndexOf(AS[i]);
                            if (closestOperator == 0)
                            {
                                if (AS[i] == '-')
                                {
                                    isNum1Negative = !isNum1Negative;
                                }
                                equation = equation.Remove(0, 1);
                                i = -1;
                                closestOperator = int.MaxValue;
                            }
                        }
                    }
                    for (int i = 0; i < AS.Length; i++)
                    {
                        if (equation.LastIndexOf(AS[i]) > closestOperator)
                        {
                            if (AS[i] == '-')
                            {
                                isNum2Negative = !isNum2Negative;
                            }
                            equation = equation.Remove(equation.LastIndexOf(AS[i]), 1);
                            i = -1;
                        }
                    }
                    int countdown = 1;
                    int countup = 1;
                    int number1 = int.Parse(equation[closestOperator - countdown].ToString());
                    int number2 = int.Parse(equation[closestOperator + countup].ToString());
                    countup++;
                    countdown++;
                    int digit = 0;
                    while (closestOperator - countdown >= 0 && int.TryParse(equation[closestOperator - countdown].ToString(), out digit))
                    {
                        digit *= power(10, (countdown - 1));
                        number1 += digit;
                        countdown++;
                    }
                    if (isNum1Negative)
                    {
                        number1 *= -1;
                    }
                    while (closestOperator + countup < equation.Length && int.TryParse(equation[closestOperator + countup].ToString(), out digit))
                    {
                        number2 *= 10;
                        number2 += digit;
                        countup++;
                    }
                    if (isNum2Negative)
                    {
                        number2 *= -1;
                    }
                    string newequation = equation.Substring(closestOperator - countdown + 1, countup + countdown - 1);
                    if (newequation.Contains('-'))
                    {
                        answer = number1 - number2;
                    }
                    else
                    {
                        answer = number1 + number2;
                    }
                    equation = equation.Replace(newequation, answer.ToString());
                    return float.Parse(equation);
                }
                else
                {
                    break;
                }
            }
            float returnvalue = 0;
            if (float.TryParse(equation, out returnvalue) == false)
            {
                errorMessageBox.Visible = true;
                SOLVERenterButton.Visible = false;
                SOLVERequationToSolve.Visible = false;
            }
            return returnvalue;
            #region NO ORDER OF OPERATIONS
            /*bool firstnumber = true;
            char operation = ' ';
            for (int a = 0; a < equation.Length;)
            {
                int number = 0;
                int nextdigit = 0;
                if (int.TryParse(equation[a].ToString(), out nextdigit))
                {
                    do
                    {
                        number *= 10;
                        number += nextdigit;
                        a++;
                        if (a >= equation.Length)
                        {
                            break;
                        }
                    }

                    while (int.TryParse(equation[a].ToString(), out nextdigit));

                    if (firstnumber)
                    {
                        answer = number;
                        firstnumber = false;
                    }
                    else
                    {
                        switch (operation)
                        {
                            case '+':
                                answer += number;
                                break;
                            case '-':
                                answer -= number;
                                break;
                            case 'x':
                            case 'X':
                            case '*':
                                answer *= number;
                                break;
                            case '/':
                            case '÷':
                                if (number == 0)
                                {
                                    //MessageBox.Show("Cannot Divide By Zero");
                                }
                                else
                                {
                                    answer /= number;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    switch (equation[a])
                    {
                        case '+':
                        case '-':
                        case 'x':
                        case 'X':
                        case '*':
                        case '/':
                        case '÷':
                            operation = equation[a];
                            break;
                    }
                    a++;
                }
            }*/
            #endregion

        }

        private int power(int number, int exponent)
        {
            int answer = number;
            for (int i = 1; i < exponent; i++)
            {
                answer *= number;
            }
            return answer;
        }
        //for GAME////////////////////////////////////////////////////////////////////////////////////////////////////

        Random random = new Random();

        int number1;
        int number2;
        char op;


        private void GAMEgiveProblemButton_Click(object sender, EventArgs e)
        {
            number1 = random.Next(0, 10);
            number2 = random.Next(0, 10);
            op = '+';

            switch (random.Next(0, 4))
            {
                case 0:
                    op = '+';
                    break;
                case 1:
                    op = '-';
                    break;
                case 2:
                    op = 'x';
                    break;
                case 3:
                    op = '÷';

                    while (number2 == 0)
                    {
                        number2 = random.Next(0, 10);
                    }

                    while (number1 % number2 != 0)
                    {
                        number1 = random.Next(0, 10);
                    }

                    break;
            }

            GAMEproblem.Text = String.Format("{0} {2} {1} = ?", number1, number2, op);

            GAMEenterButton.Visible = true;

        }


        int right = 0;
        int wrong = 0;

        int denominator;
        int numerator;


        private void GAMEmyAnswer_Click_1(object sender, EventArgs e)
        {
            GAMEmyAnswer.Text = " ";
        }

        private void GAMEenterButton_Click_1(object sender, EventArgs e)
        {
            int guess = 0;

            try
            {
                guess = Convert.ToInt32(GAMEmyAnswer.Text);
            }
            catch (FormatException)
            {
                GAMEmyAnswer.Text = "please type in a number";
                return;
            }


            int answerProblem = 0;
            switch (op)
            {
                case '+':
                    answerProblem = number1 + number2;
                    break;
                case '-':
                    answerProblem = number1 - number2;
                    break;
                case 'x':
                    answerProblem = number1 * number2;
                    break;
                case '÷':
                    answerProblem = number1 / number2;
                    break;
            }

            GAMErealAnswer.Text = answerProblem.ToString();



            if (answerProblem == guess)
            {
                right++;
                GAMEwrongRightLabel.Text = "right";
            }
            else
            {
                wrong++;
                GAMEwrongRightLabel.Text = "wrong";
            }

            GAMEnumberRight.Text = right.ToString();
            GAMEnumberWrong.Text = wrong.ToString();

            GAMEenterButton.Visible = false;
      

        }

        private void GAMEgiveScoreButton_Click_1(object sender, EventArgs e)
        {
            int grade = 0;
            numerator = Convert.ToInt32(GAMEnumberRight.Text);
            denominator = Convert.ToInt32(GAMEnumberRight.Text) + Convert.ToInt32(GAMEnumberWrong.Text);

            if (denominator == 0)
            {
                grade = 0;
            }
            else
            {
                grade = Convert.ToInt32(Convert.ToDouble(numerator) / Convert.ToDouble(denominator) * 100);
            }
            GAMEpercentLabel.Text = string.Format("{0}%", grade);

            if (grade == 100)
            {
                GAMEgrade.Text = "A+";
            }

            else if (grade >= 93 && grade <= 99)
            {
                GAMEgrade.Text = "A";
            }

            else if (grade >= 90 && grade <= 92)
            {
                GAMEgrade.Text = "A-";
            }

                //////////////////////////

            else if (grade >= 88 && grade <= 89)
            {
                GAMEgrade.Text = "B+";
            }

            else if (grade >= 83 && grade <= 87)
            {
                GAMEgrade.Text = "B";
            }

            else if (grade >= 80 && grade <= 82)
            {
                GAMEgrade.Text = "B-";
            }

                /////////////////////////

            else if (grade >= 78 && grade <= 79)
            {
                GAMEgrade.Text = "C+";
            }

            else if (grade >= 73 && grade <= 77)
            {
                GAMEgrade.Text = "C";
            }

            else if (grade >= 70 && grade <= 72)
            {
                GAMEgrade.Text = "C-";
            }

                /////////////////////////

            else if (grade >= 68 && grade <= 69)
            {
                GAMEgrade.Text = "D+";
            }

            else if (grade >= 63 && grade <= 67)
            {
                GAMEgrade.Text = "D";
            }

            else if (grade >= 60 && grade <= 62)
            {
                GAMEgrade.Text = "D-";
            }

                ////////////////////////

            else if (grade <= 59 && grade >= 0)
            {
                GAMEgrade.Text = "F";
            }
        }



        private void errorMessageBox_Click(object sender, EventArgs e)
        {
            errorMessageBox.Visible = false;
            errorMessageBox.SendToBack();
            SOLVERenterButton.Visible = true;
            SOLVERequationToSolve.Visible = true;
        }
    }
}
