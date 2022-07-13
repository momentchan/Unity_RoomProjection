namespace mj.gist.projection {
    public interface IProjectionUser {
        RoomProjection Projection { get; set; }
        void Setup(RoomProjection proj);
    }
}