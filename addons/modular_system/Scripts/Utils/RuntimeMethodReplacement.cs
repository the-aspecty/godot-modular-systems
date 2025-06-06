using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

public static class RuntimeMethodReplacement
{
    public static void ReplaceMethod(MethodInfo originalMethod, Delegate replacementDelegate)
    {
        // Get the MethodInfo from the delegate
        var replacementMethod = replacementDelegate.Method;

        // Create a dynamic method that will call our replacement
        var dynamicMethod = new DynamicMethod(
            $"{originalMethod.Name}_Replacement",
            originalMethod.ReturnType,
            new[] { originalMethod.DeclaringType },
            originalMethod.Module,
            true
        );

        var il = dynamicMethod.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0); // Load 'this'
        il.Emit(OpCodes.Call, replacementMethod);
        il.Emit(OpCodes.Ret);

        // Create a delegate from the dynamic method
        var delegateType = typeof(Action<>).MakeGenericType(originalMethod.DeclaringType);
        var wrapper = dynamicMethod.CreateDelegate(delegateType);

        RuntimeHelpers.PrepareMethod(originalMethod.MethodHandle);
        RuntimeHelpers.PrepareMethod(replacementMethod.MethodHandle);
    }
}
