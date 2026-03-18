# Class Diagram

## User
- Id (int)
- Name (string)

+ CreateUser()

## Listing
- Id (int)
- Title (string)
- Price (double)

+ CreateListing()

## Booking
- Id (int)
- UserId (int)
- ListingId (int)

+ CreateBooking()

## IRepository<T>
+ GetAll()
+ GetById(int id)
+ Add(T item)
+ Save()

## FileRepository<T>
- items (List<T>)

+ GetAll()
+ GetById(int id)
+ Add(T item)
+ Save()

## Relationships
- User → Booking (1:N)
- Listing → Booking (1:N)