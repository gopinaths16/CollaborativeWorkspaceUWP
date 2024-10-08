﻿using CollaborativeWorkspaceUWP.Models.ViewObjects.Folders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollaborativeWorkspaceUWP.Models
{
    public class Project : BaseModel, ICloneable, IFolder
    {
        private long _id;
        private string _name;
        private long _status;
        private long _priority;
        private long _teamspaceId;
        private long _ownerId;

        private bool isOpen;

        public long Id { get { return _id; } set { _id = value; } }
        public string Name { get { return _name; } set { _name = value; } }
        public long Status { get { return _status; } set { _status = value; } }
        public long Priority { get { return _priority; } set { _priority = value; } }
        public long TeamsapceId {  get { return _teamspaceId; } set { _teamspaceId = value; } }
        public long OwnerId { get { return _ownerId; } set { _ownerId = value; } }

        public ObservableCollection<Group> BoardGroups { get; set; }
        public ObservableCollection<Group> Groups { get; set; }

        public bool IsBoardView { get; set; }

        public bool IsOpen
        {
            get { return isOpen; }
            set
            {
                isOpen = value;
                NotifyPropertyChanged(nameof(IsOpen));
            }
        }

        public Project()
        {
            IsOpen = false;
        }

        public Project(long id, string name, long status, long priority, long teamspaceId, long ownerId) : base()
        {
            this._id = id;
            this._name = name;
            this._status = status;
            this._priority = priority;
            this._teamspaceId = teamspaceId;
            this._ownerId = ownerId;
            IsOpen = false;

            BoardGroups = new ObservableCollection<Group>();
            Groups = new ObservableCollection<Group>();
        }

        public object Clone()
        {
            Project project = new Project(Id, Name, Status, Priority, TeamsapceId, OwnerId);
            project.IsOpen = IsOpen;
            foreach (Group group in BoardGroups)
            {
                project.BoardGroups.Add(group);
            }
            foreach (Group group in Groups)
            {
                project.Groups.Add(group);
            }
            return project;
        }
    }
}
