namespace LFramework
{
    public delegate void CallBack();

    public delegate void CallBack<T>(T arg);

    public delegate void CallBack<T, TX>(T arg1, TX arg2);

    public delegate void CallBack<T, TX, TY>(T arg1, TX arg2, TY arg3);

    public delegate void CallBack<T, TX, TY, TZ>(T arg1, TX arg2, TY arg3, TZ arg4);

    public delegate void CallBack<T, TX, TY, TZ, TW>(T arg1, TX arg2, TY arg3, TZ arg4, TW arg5);
}