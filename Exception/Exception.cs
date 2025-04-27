using System;

namespace CarConnectExceptionLibrary
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, Exception inner) : base(message, inner) { }
    }

    public class ReservationException : Exception
    {
        public ReservationException(string message) : base(message) { }
        public ReservationException(string message, Exception inner) : base(message, inner) { }
    }

    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string message) : base(message) { }
        public VehicleNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class AdminNotFoundException : Exception
    {
        public AdminNotFoundException(string message) : base(message) { }
        public AdminNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException(string message) : base(message) { }
        public CustomerNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message) : base(message) { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
    }

    public class DatabaseConnectionException : Exception
    {
        public DatabaseConnectionException(string message) : base(message) { }
        public DatabaseConnectionException(string message, Exception inner) : base(message, inner) { }
    }
}
