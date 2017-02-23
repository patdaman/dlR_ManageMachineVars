using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;

namespace EFDataModel.DevOps
{
    public partial class DevOpsEntities : DbContext
    {
        public class CurrentState
        {
            public string tableName;
            public string fieldName;
            public string eventIdField;
            public string entityKeys;
            public object oldValue;
            public object newValue;
            public string userName;
            public string source;
            public string contentType;
        }

        public IObjectContextAdapter devOpsContext { get; private set; }

        public DevOpsEntities(string conn)
            : base(conn)
        {
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Saves the changes. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160630. </remarks>
        /// Retrieve the error messages as a list of strings.
        /// Join the list to a single string.
        /// Combine the original exception message with the new one.
        ///  Throw a new DbEntityValidationException with the improved exception message.

        /// <exception cref="DbEntityValidationException">  Thrown when a database Entity Validation
        ///                                                 error condition occurs. </exception>
        ///
        /// <returns>   An int. </returns>
        ///-------------------------------------------------------------------------------------------------

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Track changes. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160630. </remarks>
        ///
        /// <returns>   List of all changes currently within context that calls this method. </returns>
        ///-------------------------------------------------------------------------------------------------

        public IEnumerable<CurrentState> TrackChanges()
        {
            List<CurrentState> currentStateList = new List<CurrentState>();
            ChangeTracker.DetectChanges();
            ObjectContext ctx = ((IObjectContextAdapter)this).ObjectContext;

            object oldValueObject;
            object newValueObject;
            var idField = "";
            string fieldName = "";
            string entityKeys = "";
            CurrentValueRecord current;
            DbDataRecord original;

            IEnumerable<ObjectStateEntry> objectStateAddedEntryList =
                ctx.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Added).ToList();

            IEnumerable<ObjectStateEntry> objectStateEntryList =
                ctx.ObjectStateManager.GetObjectStateEntries(
                    EntityState.Modified |
                    EntityState.Deleted)
                    .ToList();

            foreach (ObjectStateEntry entry in objectStateAddedEntryList)
            {
                if (!entry.IsRelationship && entry != null)
                {
                    current = entry.CurrentValues;
                    foreach (var propertyName in current.DataRecordInfo.FieldMetadata)
                    {
                        var objectStateEntry = ctx.ObjectStateManager.GetObjectStateEntry(entry.Entity);
                        Dictionary<string, object> keys = GetAllKeyValues(entry);
                        entityKeys = getEntityKeys(keys);
                        fieldName = propertyName.FieldType.Name.ToString();
                        oldValueObject = null;
                        newValueObject = current.GetValue(
                                   current.GetOrdinal(fieldName));
                        currentStateList.Add(new CurrentState
                        {
                            userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name ?? "DevOpsEntities",
                            source = "",
                            tableName = entry.EntitySet.ToString(),
                            contentType = GetType().ToString(),
                            eventIdField = idField,
                            entityKeys = entityKeys,
                            oldValue = oldValueObject,
                            newValue = newValueObject,
                            fieldName = fieldName
                        });
                    }
                }
            }

            foreach (ObjectStateEntry entry in objectStateEntryList)
            {
                if (!entry.IsRelationship && entry != null)
                {
                    original = entry.OriginalValues;

                    foreach (var propertyName in entry.GetModifiedProperties())
                    {
                        oldValueObject = original.GetValue(
                                   original.GetOrdinal(propertyName.ToString()))
                                   ;
                        if (entry.State == EntityState.Modified)
                        {
                            current = entry.CurrentValues;
                            newValueObject = current.GetValue(
                                       current.GetOrdinal(propertyName.ToString()));
                        }
                        else
                        {
                            newValueObject = null;
                        }
                        currentStateList.Add(new CurrentState
                        {
                            userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name ?? "DevOpsEntities",
                            source = "",
                            tableName = entry.EntitySet.ToString(),
                            contentType = GetType().ToString(),
                            eventIdField = "",
                            entityKeys = getEntityKeys(entry),
                            oldValue = oldValueObject,
                            newValue = newValueObject,
                            fieldName = propertyName.ToString().Trim()
                        });
                    }
                }
            }
            return currentStateList;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets entity keys. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160630. </remarks>
        ///
        /// <param name="entry">    The entry. </param>
        ///
        /// <returns>   create formatted string of entity keys. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static string getEntityKeys(ObjectStateEntry entry)
        {
            if (entry.EntityKey.EntityKeyValues != null)
            {
                return string.Join("\",\"", entry.EntityKey.EntityKeyValues
                             .Select(x => x.Key + "\":\"" + x.Value.ToString().Replace("\"", "\\\""))).Trim();
            }
            else return null;
        }

        public static string getEntityKeys(Dictionary<string, object> keys)
        {
            if (keys != null)
            {
                return string.Join("\",\"", keys
                             .Select(x => x.Key + "\":\"" + x.Value.ToString().Replace("\"", "\\\"")));
            }
            else return null;
        }

        private Dictionary<string, object> GetAllKeyValues(ObjectStateEntry entry)
        {
            var keyValues = new Dictionary<string, object>();
            var currentValues = entry.CurrentValues;
            var keys = entry.EntityKey.EntityKeyValues;
            for (int i = 0; i < currentValues.FieldCount; i++)
            {
                if (1 == 1)
                {
                    keyValues.Add(currentValues.GetName(i), currentValues.GetValue(i));
                }
            }
            return keyValues;
        }
    }
}