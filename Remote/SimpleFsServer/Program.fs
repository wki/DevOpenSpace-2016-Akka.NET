open System
open Akka.FSharp

[<EntryPoint>]
let main argv =
    // durch die Benutzung von "actorOf2" unten wird diese Funktion
    // für jede eintreffende Nachricht aufgerufen
    let helloActor (mailbox:Actor<_>) message =
        let sender = mailbox.Context.Sender
        printfn "String '%A' received from %A" message sender
        sender <! sprintf "Answer to '%A'" message


    // Actor System und "hello" Aktor starten
    let config = Configuration.load()
    let system = System.create "Server" <| config
    let hello = spawn system "hello" <| actorOf2 helloActor

    printfn "Press [enter] to continue"
    Console.ReadLine() |> ignore

    0 // return an integer exit code
