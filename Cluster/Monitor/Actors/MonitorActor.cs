using System;
using System.Linq;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Cluster;

namespace Monitor.Actors
{
    public class MonitorActor: ReceiveActor
    {
        private const int MaxNrOfMessages = 10;

        private class Tick {}

        private Cluster cluster = Cluster.Get(Context.System);
        private Dictionary<Address, MemberStatus> nodeStatus = new Dictionary<Address, MemberStatus>();
        private Dictionary<string, HashSet<Address>> roleMembers = new Dictionary<string, HashSet<Address>>();
        private string lastAction;
        private List<string> lastMessages = new List<string>(); 

        public MonitorActor()
        {
            Context.System.Scheduler
                .ScheduleTellRepeatedly(
                    initialDelay: TimeSpan.FromSeconds(1),
                    interval: TimeSpan.FromSeconds(1),
                    receiver: Self,
                    message: new Tick(),
                    sender: ActorRefs.NoSender
                );
            
//            Receive<ClusterEvent.MemberUp>(m => HandleMemberChange(m.Member));
//            Receive<ClusterEvent.UnreachableMember>(m => HandleMemberChange(m.Member));
//            Receive<ClusterEvent.ReachableMember>(m => HandleMemberChange(m.Member));
//            Receive<ClusterEvent.MemberRemoved>(m => HandleMemberChange(m.Member));
//            Receive<ClusterEvent.MemberStatusChange>(m => HandleMemberChange(m.Member));

            Receive<ClusterEvent.IMemberEvent>(m => HandleMemberChange(m.Member, m));
            Receive<ClusterEvent.ReachabilityEvent>(m => HandleMemberChange(m.Member, m));
            Receive<ClusterEvent.IClusterDomainEvent>(m => HandleMemberChange(null, m));

            Receive<Tick>(_ => PrintStatus());

            // Receive<object>(_ => {});

            lastAction = "start";
        }

        protected override void PreStart()
        {
            cluster.Subscribe(
                Self,
                new [] { typeof(ClusterEvent.IClusterDomainEvent) }
            );
        }

        protected override void PostRestart(Exception reason)
        {
            lastAction = "restart";
        }

        protected override void PostStop()
        {
            cluster.Unsubscribe(Self);
        }

        #region event handlers
        private void HandleMemberChange(Member member, object @event)
        {
            var eventText = @event.ToString();
            var memberEvent = @event as ClusterEvent.IMemberEvent;
            if (memberEvent != null)
                eventText = eventText + $", member: {memberEvent.Member.Address}, status: {memberEvent.Member.Status}";
            var reachabilityEvent = @event as ClusterEvent.ReachabilityEvent;
            if (reachabilityEvent != null)
                eventText = eventText + $", member: {reachabilityEvent.Member.Address}, status: {reachabilityEvent.Member.Status}";
            var roleLeaderEvent = @event as ClusterEvent.RoleLeaderChanged;
            if (roleLeaderEvent != null)
                eventText = eventText + $", role: {roleLeaderEvent.Role}, leader: {roleLeaderEvent.Leader}";

            lastMessages.Add($"{DateTime.Now:hh:mm:ss.fff}: {eventText}");
            while (lastMessages.Count > MaxNrOfMessages)
                lastMessages.RemoveAt(0);

            if (member == null)
                return;

            var address = member.Address;
            if (@event is ClusterEvent.UnreachableMember)
                nodeStatus[address] = MemberStatus.Down;
            else if (@event is ClusterEvent.ReachableMember)
                nodeStatus[address] = MemberStatus.Up;
            else
              nodeStatus[address] = member.Status;

            var roles = String.Join(", ", member.Roles);
            lastAction = $"Node {address} Status {member.Status} ({roles})";

            foreach (var role in member.Roles)
            {
                if (!roleMembers.ContainsKey(role))
                    roleMembers[role] = new HashSet<Address>();

                roleMembers[role].Add(address);
            }
        }
        #endregion

        private void PrintStatus()
        {
            Console.Clear();

            Console.WriteLine("{0:hh:mm:ss}", DateTime.Now);
            Console.WriteLine();

            Console.WriteLine("Min Nr of Members: {0}", cluster.Settings.MinNrOfMembers);
            Console.WriteLine("Monitored by Nr Members: {0}", cluster.Settings.MonitoredByNrOfMembers);
            Console.WriteLine();

            Console.WriteLine("Seed Nodes:");
            cluster.Settings.SeedNodes.ForEach(s =>
                Console.WriteLine("   {0} - {1}", s, GetNodeStatus(s))
            );
            Console.WriteLine();

            Console.WriteLine("Roles - should have: {0}", String.Join(", ", cluster.Settings.Roles));
            roleMembers.Keys.ToList().ForEach(role => 
                {
                    Console.WriteLine("    {0} needed Members: {1}", role, GetNeededMembersForRole(role));
                    foreach (var member in roleMembers[role])
                        Console.WriteLine("        * {0}: {1} ", member, nodeStatus[member]);
                }
            );
            Console.WriteLine();

            Console.WriteLine("Last action: {0}", lastAction);
            Console.WriteLine();

            Console.WriteLine("Last Events:");
            lastMessages.ForEach(m => Console.WriteLine("    {0}", m));
        }

        private string GetNodeStatus(Address node)
        {
            if (nodeStatus.ContainsKey(node))
                return nodeStatus[node].ToString();
            else
                return "?";
        }

        private string GetNeededMembersForRole(string role)
        {
            if (cluster.Settings.MinNrOfMembersOfRole.ContainsKey(role))
                return cluster.Settings.MinNrOfMembersOfRole[role].ToString();
            else
                return "N/A";
        }
    }
}
