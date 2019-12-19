using MVCProjEmployees.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Service
{
    public enum GenericExceptionResponse
    {
        ConflictException,
        BadConstraintsException,
        InternalErrorException
    }
    public class GenericServiceException : Exception
    {
        public int StatusCode { get; set; }
        public GenericExceptionResponse GenericExceptionResponse { get; set; }
        public GenericServiceException()
        {

        }

        public GenericServiceException(string message) : base(message)
        {

        }

        public GenericServiceException(string message, Exception inner) : base(message, inner)
        {

        }

        public static void ConstructGenericException(Exception dbException)
        {
            var mysqlException = dbException.InnerException as MySqlException;

            if (mysqlException.Number.Equals(Constants.MySqlInvalidConstraintsErrorCode))
            {
                throw new GenericServiceException(mysqlException.Message, mysqlException.InnerException)
                { StatusCode = 400, GenericExceptionResponse = GenericExceptionResponse.BadConstraintsException };
            }
            else if (mysqlException.Number.Equals(Constants.MySqlDuplicateErrorCode))
            {
                throw new GenericServiceException(mysqlException.Message, mysqlException.InnerException)
                { StatusCode = 409, GenericExceptionResponse = GenericExceptionResponse.ConflictException };
            }
            else
            {
                throw new GenericServiceException(mysqlException.Message, mysqlException.InnerException)
                { StatusCode = 500, GenericExceptionResponse = GenericExceptionResponse.InternalErrorException };
            }
        }
    }
}
