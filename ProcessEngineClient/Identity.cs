namespace ProcessEngineClient
{
    using System;
    using System.Text;

    using EssentialProjects.IAM.Contracts;

    using IAMIdentity = EssentialProjects.IAM.Contracts.Identity;

    using ExternalTaskIdentity = ProcessEngine.ExternalTaskAPI.Contracts.Identity;

    public class Identity 
    {
        public static Identity DefaultIdentity = new Identity("dummy_token");

        internal IIdentity InternalIdentity { get; }

        internal ExternalTaskIdentity ExternalTaskIdentity { get; }

        internal Identity(string token) 
        {
            var preparedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));

            this.InternalIdentity = new IAMIdentity() { Token = preparedToken };

            this.ExternalTaskIdentity  = new ExternalTaskIdentity() { Token = preparedToken };
        }
    }
}
