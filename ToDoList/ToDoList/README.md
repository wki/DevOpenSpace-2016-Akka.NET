# Actors:

DurableActor : UntypedActor, IWithUnboundedstash
 : Basisklasse für einen persistenten Aktor
 - PersistenceId
 - Command<Class>(handler)
 - Recover<Class>(handler)


DurableView : UntypedActor
 : Basisklasse für einen persistenten View
 - Filter
 - Recover<Class>(handler)


JournalActor : ReceiveActor
 : Basisklasse für JournalWriter, JournalReader
 - StorageDir


JournalWriter : JournalActor
 --> PersistToJournal(message)


JournalReader : JournalActor
 --> RestoreFromJournal(Filter)
 <-- CompletedRestore



# Was muss passieren?

PersistToJournal --> Publish

Recover<> --> Subscribe


# Verbindung zum View

SignalR

