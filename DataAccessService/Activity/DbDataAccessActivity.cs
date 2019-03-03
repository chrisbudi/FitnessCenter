using System;
using System.Data.Entity.Validation;
using System.Linq;
using DataAccessService.Registrasi;
using DataObjects.Context;

namespace DataAccessService.Activity
{
    public class DbDataAccessActivity : DbDataAccessRegistrasi
    {
        protected FitEntity _db = null;

        protected DbDataAccessActivity()
        {
            _db = new FitEntity();
        }

        protected new void Save()
        {
            try
            {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString(), ex.InnerException);
            }
        }
    }
}
