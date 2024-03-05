using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LogLevel
{
    /**
     * Log for situations that cannot be recovered from.
     */
    Fatal,
    /**
     * Log for sitautions when a recoverable problem has occurred.
     */
    Error,
    /**
     * Log for situations where something is possibly wrong.
     */
    Warn,
    /**
     * Log for information about the normal execution of the application.
     */
    Info,
    /**
     * Log for more detailed information about the execution of the application.
     */
    Debug,
    /**
     * Log for very fine-grained information about the execution of the application.
     */
    Trace
}
