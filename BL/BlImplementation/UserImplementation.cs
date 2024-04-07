
namespace BlImplementation
{
    using System.Linq;
    using System.Collections.Generic;
    using BO;
    using BlApi;
    using System;

    internal class UserImplementation : IUser
    {
        private DalApi.IDal _dal = DalApi.Factory.Get;
        /// <summary>
        /// Creates a new engineer in the system.
        /// </summary>
        /// <param name="boEng">The engineer object to create.</param>
        /// <returns>The ID of the newly created engineer.</returns>
        /// <exception cref="BO.Exceptions.BlNullPropertyException">Thrown when the engineer object is null.</exception>
        /// <exception cref="BO.Exceptions.BlInvalidValueException">Thrown when the engineer object contains invalid values.</exception>
        /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException">Thrown when trying to create an engineer that already exists in the system.</exception>
        public int Create(BO.User boUser)
        {
            if (boUser == null)
                throw new BO.BlNullPropertyException("User is null");
            if (boUser.Role == UserRole.Engineer)
            {
                DO.Engineer userEng = _dal.Engineer.Read(boUser.Id);
                if (userEng == null) 
                {
                    throw new BO.BlCreationImpossibleException($"Engineer with ID={boUser.Id} does not exists");
                }
                boUser.Name = userEng.Name;
            }
            DO.User doUser = new DO.User()
            {
                Id = boUser.Id,
                Name = boUser.Name,
                Password = boUser.Password,
                Role=(DO.UserRole)boUser.Role,
            };
            try
            {
                int idUser = _dal.User.Create(doUser);
                return idUser;
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlDalAlreadyExistsException($"User with ID={boUser.Id} already exists", ex);
            }
        }
        /// <summary>
        /// Deletes an engineer from the system.
        /// </summary>
        /// <param name="id">The ID of the engineer to delete.</param>
        /// <exception cref="BO.Exceptions.BlDalDeletionImpossible">Thrown when deletion of the engineer is not possible due to ongoing tasks.</exception>
        /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the engineer with the specified ID does not exist.</exception>
        public void Delete(int id)
        {
            BO.User? boUser = Read(id);
            if (boUser != null)
            {
                try
                {
                    _dal.User.Delete(id);
                }
                catch (DO.DalDoesNotExistException ex)
                {
                    throw new BO.BlDoesNotExistException($"User with ID={id} does not exist", ex);
                }
            }
        }
        /// <summary>
        /// Reads an engineer from the system based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the engineer to read.</param>
        /// <returns>The engineer object if found, otherwise null.</returns>
        /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the engineer with the specified ID does not exist.</exception>
        public BO.User? Read(int id)
        {
            DO.User? doUser = _dal.User.Read(id);
            if (doUser == null)
                throw new BO.BlDoesNotExistException($"User with ID={id} does Not exist");

            return new BO.User()
            {
                Id = id,
                Password = doUser.Password,
                Role = (BO.UserRole)doUser.Role,
            };
        }
        /// <summary>
        /// Reads all engineers from the system based on an optional filter.
        /// </summary>
        /// <param name="filter">An optional filter predicate to apply on the engineers.</param>
        /// <returns>An enumerable collection of engineers.</returns>
        public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter)
        {
            if (filter != null)
                return (from DO.User doUser in _dal.User.ReadAll()
                        let boUser = Read(doUser.Id)
                        where filter(boUser)
                        select boUser);

            return (
                from DO.User doUser in _dal.User.ReadAll()
                let boUser = Read(doUser.Id)
                select boUser);
        }
        /// <summary>
        /// Updates an existing engineer in the system.
        /// </summary>
        /// <param name="boEng">The engineer object containing updated information.</param>
        /// <exception cref="BO.Exceptions.BlNullPropertyException">Thrown when the engineer object is null.</exception>
        /// <exception cref="BO.Exceptions.BlInvalidValueException">Thrown when the engineer object contains invalid values.</exception>
        /// <exception cref="BO.Exceptions.BlDoesNotExistException">Thrown when the engineer to update does not exist.</exception>
        /// <exception cref="BO.Exceptions.BlDalAlreadyExistsException">Thrown when trying to update an engineer that already exists in the system.</exception>
        public void Update(BO.User boUser)
        {
            if (boUser == null)
                throw new BO.BlNullPropertyException("User to update is null");
            DO.User? doUser = _dal.User.Read(boUser.Id);
            if (doUser == null)
                throw new BO.BlDoesNotExistException($"User with ID={boUser!.Id} does Not exist");
            try
            {
                _dal.User.Update(new DO.User()
                {
                    Id = boUser.Id,
                    Password = boUser.Password,
                    Role = (DO.UserRole)boUser.Role,
                });
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlDalAlreadyExistsException($"User with ID={boUser!.Id} already exists", ex);
            }
            catch (DO.DalDoesNotExistException ex)
            {
                throw new BO.BlDoesNotExistException($"User with ID={boUser!.Id} does Not exist", ex);
            }
        }
    }
}