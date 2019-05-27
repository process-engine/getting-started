namespace ProcessEngineClient
{
    using System;
    using System.Text;

    using EssentialProjects.IAM.Contracts;

    using IAMIdentity = EssentialProjects.IAM.Contracts.Identity;

    public class Identity 
    {
        public static Identity DefaultIdentity = new Identity("dummy_token");

        internal IIdentity InternalIdentity { get; }

        internal Identity(string token) 
        {
            this.InternalIdentity = new IAMIdentity() { Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(token)) };
        }
    }
}