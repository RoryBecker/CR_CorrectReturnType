CR_CorrectReturnType
====================

This CodeRush plugin adds a new CodeProvider called '**Correct Return Type**'

This feature is available from return statements whose type differs from that of the function that contains them.

When activated, **Correct Return Type** will determine the type of the expression and alter the function to return that type. 

Given the following function... 

    private int GetName()
    {
        return "Rory";
    }

 * Place your cater on the '**return**' keyword.
 * Invoke '**Correct Return Type**'.

CodeRush will alter your function to read..

    private string GetName()
    {
        return "Rory";
    }
 
