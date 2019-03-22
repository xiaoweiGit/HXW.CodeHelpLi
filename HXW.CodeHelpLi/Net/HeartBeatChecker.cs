// ***************************************************************
// version:  2.0    date: 04/08/2008
//  -------------------------------------------------------------
// 
//  -------------------------------------------------------------
// previous version:  1.4    date: 05/11/2006
//  -------------------------------------------------------------
//  (C) 2006-2008   HXW.CodeHelpLi All Rights Reserved
// ***************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace  HXW.CodeHelpLi.Net
{
    /// <summary>
    /// ���������
    /// </summary>
    public abstract class  HeartBeatChecker : DisposableAndStartableBase
    {
        protected Timer checkTimer;
        int heartBeatPeriod;
        bool enableCheckHeartBeat;

        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public HeartBeatChecker()
        {
            heartBeatPeriod = 30000; //Ĭ��ʱ����30��
        }

        /// <summary>
        /// �Ƿ�����������鹦��
        /// </summary>
        public bool EnableCheckHeartBeat
        {
            get { return enableCheckHeartBeat; }
            set { enableCheckHeartBeat = value; }
        }

        /// <summary>
        /// �������ʱ����(����)
        /// </summary>
        public int HeartBeatPeriod
        {
            get { return heartBeatPeriod; }
            set { heartBeatPeriod = value; }
        }

        /// <summary>
        /// ����������鹦��
        /// </summary>
        protected override void OnStart()
        {
            if (EnableCheckHeartBeat)
            {
                {
                    checkTimer = new Timer(new TimerCallback(CheckHeartBeatCallBack), null,
                        HeartBeatPeriod, HeartBeatPeriod);
                    NetDebuger.PrintDebugMessage("Start heartbeat checker, Period:" + HeartBeatPeriod + "(ms)");
                }
            }
        }

        /// <summary>
        /// ֹͣ�����������
        /// </summary>
        protected override void OnStop()
        {
            if (EnableCheckHeartBeat && checkTimer != null)
            {
                lock (checkTimer)
                {
                    if (EnableCheckHeartBeat && checkTimer != null)
                    {
                        NetDebuger.PrintDebugMessage("Stop heartbeat checker");
                        checkTimer.Dispose();
                        checkTimer = null;
                    }
                }
            }
        }

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        /// <param name="dispodedByUser"></param>
        protected override void Free(bool dispodedByUser)
        {
            if (dispodedByUser)
            {
                Stop();
            }

            base.Free(dispodedByUser);
        }

        /// <summary>
        /// ��������Ļص�����
        /// </summary>
        /// <param name="o">������δʹ�ã�</param>
        protected abstract void CheckHeartBeatCallBack(object o);
    }
}