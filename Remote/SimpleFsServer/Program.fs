open Akka.FSharp

// durch die Benutzung von "actorOf2" unten wird diese Funktion
// für jede eintreffende Nachricht aufgerufen
let helloActor (mailbox:Actor<_>) message =
    let sender = mailbox.Context.Sender
    printfn "String '%s' received from %A" message sender
    sender <! sprintf "Answer to '%s'" message

[<EntryPoint>]
let main argv =
    // Actor System und "hello" Aktor starten
    use system = System.create "Server" <| Configuration.load()
    let hello = spawn system "hello" <| actorOf2 helloActor

    printfn "Press [enter] to continue"
    System.Console.ReadLine() |> ignore

    0 // return an integer exit code
