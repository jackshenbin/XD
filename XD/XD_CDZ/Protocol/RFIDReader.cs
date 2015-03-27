using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RFIDREAD
{
    public class CardInfo
    {
        public string cardId;
        public bool bLockCard;
        public bool enableKey;
        public int money;
        public int factoryType;
        public string softVersion;
        public string userPass;
    }
    public class RFIDReader
    {
        public static bool  IsOldCard()
        {
            int comHandle = OpenUsb();
            GetConfig(comHandle);


            SetConfig(comHandle);

            int ret1;
            string uid;
            GetUID(comHandle, out ret1, out uid);
            if (string.IsNullOrEmpty(uid))
            {
                GetUID(comHandle, out ret1, out uid);
            }
            if (string.IsNullOrEmpty(uid))
                throw new Exception( "无法获取uid");

            ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
            block.block = new byte[0xd0];
            block.uid.uid = new byte[10];
            block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];
            block.num = 1;

            block.addr = 0;
            if (protocol.tagMode == 0)
            {
                block.keyType = 0x60;
                //memcpy(block.key, block1Key, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block1Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
            }
            else
            {
                block.keyType = 0x61;
                //memcpy(block.key, block1Key + HFREADER_ISO14443A_LEN_M1BLOCK - HFREADER_ISO14443A_LEN_M1_KEY, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block1Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
            }
            protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
            if (block.result.flag > 0)
            {
                return false ;

            }
            CloseUsb(comHandle);

            return true;
        }
        public static string ReadUID()
        {
            Trace.WriteLine("enter ReadUID");

            int comHandle = OpenUsb();
            if (comHandle > 0)
            {

                GetConfig(comHandle);


                SetConfig(comHandle);

                int ret;
                string uid;
                GetUID(comHandle, out ret, out uid);
                if (string.IsNullOrEmpty(uid))
                {
                    GetUID(comHandle, out ret, out uid);
                }
                ret = CloseUsb(comHandle);
                Trace.WriteLine("leave ReadUID");

                return uid;
            }
            throw new Exception( "无法打开设备，请确认读卡器以及正常连接，error:" + comHandle.ToString());
            
        }
        public static CardInfo ReadCardInfo()
        {
            string cardId;
            bool bLockCard;
            bool enableKey;
            int money;
            int factoryType;
            string softVersion;
            string userPass;

            int comHandle = OpenUsb();
            GetConfig(comHandle);


            SetConfig(comHandle);

            int ret1;
            string uid;
            GetUID(comHandle, out ret1, out uid);
            if (string.IsNullOrEmpty(uid))
            {
                GetUID(comHandle, out ret1, out uid);
            }
            if (string.IsNullOrEmpty(uid))
                throw new Exception("无法获取uid");

            ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
            block.block = new byte[0xd0];
            block.uid.uid = new byte[10];
            block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];
            block.num = 1;

            block.addr = 1;
            if (protocol.tagMode == 0)
            {
                block.keyType = 0x60;
                //memcpy(block.key, block1Key, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block1Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
            }
            else
            {
                block.keyType = 0x61;
                //memcpy(block.key, block1Key + HFREADER_ISO14443A_LEN_M1BLOCK - HFREADER_ISO14443A_LEN_M1_KEY, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block1Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
            }
            protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
            if (block.result.flag > 0)
            {
                throw new Exception("读取卡号失败！");


            }
            else
            {
                cardId = getTextFromeHex(block.block, protocol.HFREADER_ISO14443A_LEN_M1BLOCK);
                //int i = 0;
                //for(i = 0; i < HFREADER_ISO14443A_LEN_M1BLOCK; i++)
                //{
                //    if(block.block[i] > 9)
                //    {
                //        if(bDisplayErrorInf)
                //        {
                //            QMessageBox::information(this, QString(tr("提示")), QString(tr("卡号错误，请先退卡！")));
                //        }
                //        return -2;
                //    }
                //}
                //getTextFromeDec(block.block, HFREADER_ISO14443A_LEN_M1BLOCK, cardId);
            }


            block.addr = 16;
            if (protocol.tagMode == 0)
            {
                block.keyType = 0x60;
                //memcpy(block.key, block16Key, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block16Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);

            }
            else
            {
                block.keyType = 0x61;
                //emcpy(block.key, block16Key + HFREADER_ISO14443A_LEN_M1BLOCK - HFREADER_ISO14443A_LEN_M1_KEY, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block16Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);

            }
            protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
            if (block.result.flag > 0)
            {
                throw new Exception("读取卡状态失败！");
            }
            else
            {
                userPass = getUserPassFromHex(block.block, 8);
                if (block.block[8] == 0xAA)
                {
                    bLockCard = true;
                }
                else
                {
                    bLockCard = false;
                }

                if (block.block[9] == 0xAA)
                {
                    enableKey = true;
                }
                else
                {
                    enableKey = false;
                }
                factoryType = block.block[10];
                softVersion = (Convert.ToInt32(block.block[11]) / 100f).ToString("0.00");
            }

            block.addr = 32;
            if (protocol.tagMode == 0)
            {
                block.keyType = 0x60;
                //memcpy(block.key, block32Key, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block32Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);

            }
            else
            {
                block.keyType = 0x61;
                //memcpy(block.key, block32Key + HFREADER_ISO14443A_LEN_M1BLOCK - HFREADER_ISO14443A_LEN_M1_KEY, HFREADER_ISO14443A_LEN_M1_KEY);
                Array.Copy(protocol.block32Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);

            }
            protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
            if (block.result.flag > 0)
            {
                throw new Exception("读取卡余额失败！");
            }
            else
            {
                int money1 = block.block[0] + (block.block[1] << 8) + (block.block[2] << 16) + (block.block[3] << 24);
                int money2 = block.block[4] + (block.block[5] << 8) + (block.block[6] << 16) + (block.block[7] << 24);
                int money3 = block.block[8] + (block.block[9] << 8) + (block.block[10] << 16) + (block.block[11] << 24);
                byte addr1 = block.block[12];
                byte addr2 = block.block[13];
                byte addr3 = block.block[14];
                byte addr4 = block.block[15];


                //UCHAR moneyArray[4] = {0};
                //unsigned int d1 = 0, d2 = 0, d3 = 0;
                //UCHAR addr1 = 0, addr2 = 0, addr3 = 0, addr4 = 0;
                //float m = 0;

                //memcpy(moneyArray, block.block + 0, 4);
                //d1 = *((unsigned int *)(moneyArray));
                //memcpy(moneyArray, block.block + 4, 4);
                //d2 = *((unsigned int *)(moneyArray));
                //memcpy(moneyArray, block.block + 8, 4);
                //d3 = *((unsigned int *)(moneyArray));
                //byte addr1 = block.block[12];
                //byte addr2 = block.block[13];
                //byte addr3 = block.block[14];
                //byte addr4 = block.block[15];

                if (protocol.tagMode == 0)
                {
                    if (money1 != (~money2))
                    {
                        throw new Exception("金额错误，请先退卡！");
                    }
                }
                else
                {
                    //unsigned int d22 = ~d2;
                    //UCHAR addr22 = ~addr2;
                    if ((money1 != (~money2)) || (money1 != money3) || (addr1 != 0x20) || (addr2 != 0xdf) || (addr3 != 0x20) || (addr4 != 0xdf))
                    {
                        throw new Exception("金额错误，请先退卡！");
                    }
                }
                money = money1;
            }
            CloseUsb(comHandle);
            CardInfo info = new CardInfo()
            {
                cardId = cardId,
                money = money,
                bLockCard = bLockCard,
                enableKey = enableKey,
                factoryType = factoryType,
                softVersion = softVersion,
                userPass = userPass,
            };
            return info;
        }

        public static string NewCard(string cardid, string userpass, bool pKeyCheck)
        {
            Trace.WriteLine("enter NewCard");
            string retCard = cardid;
            int comHandle = OpenUsb();
            GetConfig(comHandle);


            SetConfig(comHandle);

            int ret1;
            string uid;
            GetUID(comHandle, out ret1, out uid);
            if (string.IsNullOrEmpty(uid))
            {
                GetUID(comHandle, out ret1, out uid);
            }
            if (string.IsNullOrEmpty(uid))
                throw new Exception( "无法获取uid");

            byte[] p = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

            byte[] pwd = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];
            Array.Copy(p,0,pwd,0,Math.Min( p.Length,protocol.HFREADER_ISO14443A_LEN_M1_KEY));

            if (comHandle > 0)
            {
                byte[] pTxFrame = new byte[0x400];
                byte[] pRxFrame = new byte[0x400];

                ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
                block.uid = new ISO14443A_UID();
                block.uid.uid = new byte[10];
                block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY] ;
                pwd.CopyTo(block.key,0);
                block.keyType = 0x60;
                block.num = 1;

                block.addr = 1;
                byte[] bcard = gethexFromeText(cardid);
                block.block = new byte[0xd0];
                bcard.CopyTo(block.block, 0);

                int ret = 0;
                protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                if ( block.result.flag == 0)
                {
                    int pos =0;
                    block.addr = 16;
                    block.block[pos++] = 0xFF;
		            block.block[pos++] = 0xFF;
                    byte[] bpass = getpassbyteFromeText(userpass);
                    bpass.CopyTo(block.block,pos);
		            pos += protocol.USER_PASSWORD_LENGTH;
		            block.block[pos++] = 0x55;
                    if (pKeyCheck)
		            {
			            block.block[pos++] = 0xAA;
		            }
		            else
		            {
			            block.block[pos++] = 0x55;
		            }
		            block.block[pos++] = 0x01;
		            block.block[pos++] = 0x64;
		            block.block[pos++] = 0xFF;
		            block.block[pos++] = 0xFF;
		            block.block[pos++] = 0xFF;
		            block.block[pos++] = 0xFF;
                    pTxFrame = new byte[0x400];
                    pRxFrame = new byte[0x400];

                     protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                    if ( block.result.flag == 0)
                    {
                        pos = 0;
                        block.addr = 32;
                        if (protocol.tagMode == 0)
                        {
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                        }
                        else
                        {
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0xFF;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x00;
                            block.block[pos++] = 0x20;
                            block.block[pos++] = 0xDF;
                            block.block[pos++] = 0x20;
                            block.block[pos++] = 0xDF;
                        }
                        pTxFrame = new byte[0x400];
                        pRxFrame = new byte[0x400];

                        protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                        if(block.result.flag !=0)
			            {
				            throw new Exception( "写金额失败！");//QMessageBox::information(this, QString(tr("提示")), QString(tr("写金额失败！")));
                            ret = 1;
			            }


                    }
                    else
                    {
                        throw new Exception( "写用户密码失败！");
	                    //QMessageBox::information(this, QString(tr("提示")), QString(tr("写用户密码失败！")));
                        ret = 1;
                    }


                }	
                else
	            {
                        throw new Exception( "写卡号失败！");
		            //QMessageBox::information(this, QString(tr("提示")), QString(tr("写卡号失败！")));
                        ret = 1;
                }


                if(ret==0)
                {
                    pwd.CopyTo(block.key,0);
		            block.keyType = 0x60;
		            block.num = 1;

		            block.addr = 3;
                    Array.Copy(protocol.block1Key, 0, block.block, 0, protocol.HFREADER_ISO14443A_LEN_M1BLOCK);
                    pTxFrame = new byte[0x400];
                    pRxFrame = new byte[0x400];

                    protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
		            if(block.result.flag != 0)
		            {
                        throw new Exception(  "修改卡号区密码失败！");
			            //QMessageBox::information(this, QString(tr("提示")), QString(tr("修改卡号区密码失败！")));
                        ret = 1;
                    }
		            else
                    {
			            block.addr = 19;
                        Array.Copy(protocol.block16Key, 0, block.block, 0, protocol.HFREADER_ISO14443A_LEN_M1BLOCK);
                        pTxFrame = new byte[0x400];
                        pRxFrame = new byte[0x400];
                         protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
			            if(block.result.flag != 0)
			            {
                        throw new Exception( "修改用户密码区密码失败！");
				            //QMessageBox::information(this, QString(tr("提示")), QString(tr("修改用户密码区密码失败！")));
                        ret = 1;
                        }
			            else
			            {
				            block.addr = 35;
                            Array.Copy(protocol.block32Key, 0, block.block, 0, protocol.HFREADER_ISO14443A_LEN_M1BLOCK);
                            pTxFrame = new byte[0x400];
                            pRxFrame = new byte[0x400];
                             protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
				            if(block.result.flag != 0)
				            {
                        throw new Exception( "修改金额区密码失败！");
					            //QMessageBox::information(this, QString(tr("提示")), QString(tr("修改金额区密码失败！")));
                        ret = 1;
                            }
			            }
		            }
	            }



            }
            else
            {
                        throw new Exception( "初始化HFreader失败！");

            }
            ret1 = CloseUsb(comHandle);
            Trace.WriteLine("leave NewCard:"+retCard);


            return  retCard ;
        }

        public static string ReCharge(int chargeMoney)
        {

            string retCharge = chargeMoney.ToString();

            if (chargeMoney > 0)
            {
                int comHandle = OpenUsb();
                GetConfig(comHandle);


                SetConfig(comHandle);

                int ret1;
                string uid;
                GetUID(comHandle, out ret1, out uid);
                if (string.IsNullOrEmpty(uid))
                {
                    GetUID(comHandle, out ret1, out uid);
                }

                int err = 0;
                int errorCode = 0;

                ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
                block.block = new byte[0xd0];
                block.uid.uid = new byte[10];
                block.addr = 32;
                block.num = 1;

                block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];

                if (protocol.tagMode == 0)
                {
                    Array.Copy(protocol.block32Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    block.keyType = 0x60;
                }
                else
                {
                    Array.Copy(protocol.block32Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    block.keyType = 0x61;
                }
                byte[] pTxFrame = new byte[0x400];
                byte[] pRxFrame = new byte[0x400];

                 protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                errorCode = (int)block.result.errType;
                //saveLog(QString("Charge readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                if ( block.result.flag == 0)
                {

                    //充值
                    int d = Marshal.ReadInt32(block.block, 0);
                    int m1 = chargeMoney ;
                    errorCode = 0;

                    d += m1;

                    Marshal.WriteInt32(block.block, 0, d);
                    Marshal.WriteInt32(block.block, 4, ~d);
                    if (protocol.tagMode == 1)
                    {
                        Marshal.WriteInt32(block.block, 8, d);
                        block.block[12] = 0x20;
                        block.block[13] = 0xDF;
                        block.block[14] = 0x20;
                        block.block[15] = 0xDF;
                    }

                     protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                    errorCode = (int)block.result.errType;
                    //saveLog(QString("Charge readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                    if ( block.result.flag == 0)
                    {
                        float f = d / 100.0f;
                        retCharge = "充值成功！"+f.ToString();
                    }
                    else
                    {
                        throw new Exception( "充值失败！");
                    }
                }
                else
                {
                    throw new Exception( "读取余额失败！");
                }

                ret1 = CloseUsb(comHandle);
            }
            else
            {
                throw new Exception( "充值金额需大于0元！");
            }
                Trace.WriteLine("leave Charge:"+retCharge);

            return retCharge;
        }
        public static string Charge(int chargeMoney)
        {

            //获取充值金额
            string retCharge = chargeMoney.ToString();

            if (chargeMoney >= 0)
            {
                int comHandle = OpenUsb();
                GetConfig(comHandle);


                SetConfig(comHandle);

                int ret1;
                string uid;
                GetUID(comHandle, out ret1, out uid);
                if (string.IsNullOrEmpty(uid))
                {
                    GetUID(comHandle, out ret1, out uid);
                }

                int err = 0;
                int errorCode = 0;

                ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
                block.block = new byte[0xd0];
                block.uid.uid = new byte[10];
                block.addr = 32;
                block.num = 1;

                block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];

                if (protocol.tagMode == 0)
                {
                    Array.Copy(protocol.block32Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    block.keyType = 0x60;
                }
                else
                {
                    Array.Copy(protocol.block32Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    block.keyType = 0x61;
                }
                byte[] pTxFrame = new byte[0x400];
                byte[] pRxFrame = new byte[0x400];

                 protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                errorCode = (int)block.result.errType;
                //saveLog(QString("Charge readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                if (block.result.flag == 0)
                {
                    //扣费
                    int d = Marshal.ReadInt32(block.block, 0);

                    if (d > 0)
                    {
                        int m1 = chargeMoney ;
                        errorCode = 0;

                        d -= m1;

                        Marshal.WriteInt32(block.block, 0, d);
                        Marshal.WriteInt32(block.block, 4, ~d);
                        if (protocol.tagMode == 1)
                        {
                            Marshal.WriteInt32(block.block, 8, d);
                            block.block[12] = 0x20;
                            block.block[13] = 0xDF;
                            block.block[14] = 0x20;
                            block.block[15] = 0xDF;
                        }

                         protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                        errorCode = (int)block.result.errType;
                        //saveLog(QString("Charge readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                        if (block.result.flag == 0)
                        {
                            float f = d / 100.0f;
                            retCharge = "扣费成功！" + f.ToString();
                        }
                        else
                        {
                            throw new Exception("扣费失败！");
                        }
                    }
                    else
                    {
                        throw new Exception( "欠费卡，请充值！");
                    }


                }
                ret1 = CloseUsb(comHandle);
            }
            else
            {
                throw new Exception( "扣费金额不可以是0元！");
            }
                Trace.WriteLine("leave consump:"+retCharge);

            return retCharge;
        }

        public static string ChangePassword(string userpass, bool pKeyCheck)
        {
            Trace.WriteLine("enter ChangePassword");

            int comHandle = OpenUsb();
            GetConfig(comHandle);


            SetConfig(comHandle);

            int ret1;
            string uid;
            GetUID(comHandle, out ret1, out uid);
            if (string.IsNullOrEmpty(uid))
            {
                GetUID(comHandle, out ret1, out uid);
            }
            if (string.IsNullOrEmpty(uid))
                throw new Exception("无法获取uid");

            byte[] p = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

            byte[] pwd = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];
            Array.Copy(p, 0, pwd, 0, Math.Min(p.Length, protocol.HFREADER_ISO14443A_LEN_M1_KEY));

            if (comHandle > 0)
            {
                byte[] pTxFrame = new byte[0x400];
                byte[] pRxFrame = new byte[0x400];

                ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
                block.uid = new ISO14443A_UID();
                block.uid.uid = new byte[10];
                block.block = new byte[0xd0];

                block.num = 1;

                block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];

                if (protocol.tagMode == 0)
                {
                    Array.Copy(protocol.block16Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    block.keyType = 0x60;
                }
                else
                {
                    Array.Copy(protocol.block16Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    block.keyType = 0x61;
                }



                block.addr = 16;
                protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                if (block.result.flag > 0)
                {
                    throw new Exception("读取卡状态失败！");
                }
                else
                {
                int pos = 0;

                block.block[pos++] = 0xFF;
                block.block[pos++] = 0xFF;
                byte[] bpass = getpassbyteFromeText(userpass);
                bpass.CopyTo(block.block, pos);
                pos += protocol.USER_PASSWORD_LENGTH;
                pos++;
                if (pKeyCheck)
                {
                    block.block[pos++] = 0xAA;
                }
                else
                {
                    block.block[pos++] = 0x55;
                }

                protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                if (block.result.flag == 0)
                {

                }
                else
                {
                    throw new Exception("写用户密码失败！");
                    //QMessageBox::information(this, QString(tr("提示")), QString(tr("写用户密码失败！")));

                }

                }




            }
            else
            {
                throw new Exception("初始化HFreader失败！");

            }
            ret1 = CloseUsb(comHandle);
            Trace.WriteLine("leave ChangePassword:" + uid);


            return uid;
        }
        
        public static string QuitCard()
        {
            int comHandle = OpenUsb();

            GetConfig(comHandle);


            SetConfig(comHandle);

            int ret1;
            string uid;
            GetUID(comHandle, out ret1, out uid);
            if (string.IsNullOrEmpty(uid))
            {
                GetUID(comHandle, out ret1, out uid);
            }

            string ret = uid;
            int err = 0;
            uint errorCode = 0;

            byte[] defauleKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x07, 0x80, 0x69, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
            block.block = new byte[0xd0];
            block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];
            block.uid.uid = new byte[10];
            block.num = 1;
            Array.Copy(defauleKey, block.block, protocol.HFREADER_ISO14443A_LEN_M1BLOCK);

            block.addr = 3;
            if (protocol.tagMode == 0)
            {
                block.keyType = 0x60;
                Array.Copy(protocol.block1Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
            }
            else
            {
                block.keyType = 0x61;
                Array.Copy(protocol.block1Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
            }
             protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
            errorCode = block.result.errType;
            //saveLog(QString("Quit readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
            if ( block.result.flag == 0)
            {
                block.addr = 19;
                if (protocol.tagMode == 0)
                {
                    block.keyType = 0x60;
                    Array.Copy(protocol.block16Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                }
                else
                {
                    block.keyType = 0x61;
                    Array.Copy(protocol.block16Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                }

                 protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                errorCode = block.result.errType;
                //saveLog(QString("Quit readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                if ( block.result.flag == 0)
                {
                    block.addr = 35;
                    if (protocol.tagMode == 0)
                    {
                        block.keyType = 0x60;
                        Array.Copy(protocol.block32Key, block.key, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    }
                    else
                    {
                        block.keyType = 0x61;
                        Array.Copy(protocol.block32Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);
                    }

                     protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                    errorCode = block.result.errType;
                    //saveLog(QString("Quit readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                    if ( block.result.flag != 0)
                    {
                        throw new Exception( "恢复卡金额区密码失败！");
                        err = 1;
                    }
                }
                else
                {
                    throw new Exception( "恢复卡用户密码区密码失败！");
                        err = 1;
                }
            }
            else
            {
                throw new Exception( "恢复卡号区密码失败");
                        err = 1;
            }

            if (err == 0)
            {

                block = new ISO14443A_BLOCKPARAM();
                block.block = new byte[0xd0];
                block.key = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
                block.uid.uid = new byte[10];

                block.keyType = 0x60;
                block.num = 1;

                block.addr = 1;
                 protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                block.addr = 2;
                 protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                errorCode = block.result.errType;
                //saveLog(QString("Quit readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                if ( block.result.flag == 0)
                {
                    block.addr = 16;
                     protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                    block.addr = 17;
                     protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                    block.addr = 18;
                     protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                    errorCode = block.result.errType;
                    //saveLog(QString("Quit readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                    if ( block.result.flag == 0)
                    {
                        block.addr = 32;
                         protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                        block.addr = 33;
                         protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                        block.addr = 34;
                         protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                        errorCode = block.result.errType;
                        //saveLog(QString("Quit readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                        if ( block.result.flag != 0)
                        {
                            throw new Exception( "恢复卡金额区失败！");
                        }
                    }
                    else
                    {
                        throw new Exception( "恢复卡用户密码区失败！");
                    }
                }
                else
                {
                    throw new Exception( "恢复卡号区失败");
                }
            }

            ret1 = CloseUsb(comHandle);
            Trace.WriteLine("leave QuitCard:"+ret);

            return ret;
        }

        public static string UnLock()
        {
            string ret = "";
            int comHandle = OpenUsb();
            GetConfig(comHandle);


            SetConfig(comHandle);

            int ret1;
            string uid;
            GetUID(comHandle, out ret1, out uid);
            if (string.IsNullOrEmpty(uid))
            {
                GetUID(comHandle, out ret1, out uid);
            }

            int err = 0;
            uint errorCode = 0;

            ISO14443A_BLOCKPARAM block = new ISO14443A_BLOCKPARAM();
            block.block = new byte[0xd0];
            block.uid.uid = new byte[10];
            block.key = new byte[protocol.HFREADER_ISO14443A_LEN_M1_KEY];

            if (protocol.tagMode == 0)
            {
                block.key = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                block.keyType = 0x60;
            }
            else
            {
                Array.Copy(protocol.block16Key, protocol.HFREADER_ISO14443A_LEN_M1BLOCK - protocol.HFREADER_ISO14443A_LEN_M1_KEY, block.key, 0, protocol.HFREADER_ISO14443A_LEN_M1_KEY);

                block.keyType = 0x61;
            }
            block.addr = 16;
            block.num = 1;
             protocol.iso14443AAuthReadM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);

            //err = readerAuthReadM1Block(comHandle, 0, 1, &block, NULL, NULL);
            errorCode = block.result.errType;
            //saveLog(QString("Resume readerAuthReadM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
            if (block.result.flag == 0)
            {
                //恢复卡
                block.block[8] = 0x55;

                 protocol.iso14443AAuthWriteM1Block(comHandle, 0, 1, ref block, IntPtr.Zero, IntPtr.Zero);
                errorCode = block.result.errType;
                //saveLog(QString("Resume readerAuthWriteM1Block:err=%1 errorCode=%2").arg(err, 2, 16, QLatin1Char('0')).arg(errorCode, 2, 16, QLatin1Char('0')));
                if (block.result.flag == 0)
                {
                    ret = "解冻卡成功！";
                }
                else
                {
                    throw new Exception( "解冻卡失败！");
                }
            }
            else
            {
                throw new Exception( "读取冻结信息失败！");
            }

            ret1 = CloseUsb(comHandle);
            Trace.WriteLine("leave resume");

            return ret;
        }
        static byte[] getpassbyteFromeText(string s)
        {
            char[] c = s.ToCharArray();
            byte[] b = new byte[c.Length];
            for (int i = 0; i < s.Length; i++)
            {
                b[i] = (byte)(c[i]-0x30);
            } return b;
        }

        static byte[] gethexFromeText(string s)
        {
                   char[] c= s.ToCharArray();
                   byte[] b = new byte[c.Length];
                    for(int i=0;i<c.Length;i++)
                    {
                        b[i] = (byte)c[i];
                    } return b;
        }
        static string getTextFromeHex(byte[] b, uint len)
        {
            string ret = "";
            for (int i = 0; i < len; i++)
            {
                ret += (char)b[i];
            }

            return ret;
        }
        static byte[] getBCDFromeText(string s)
        {
                   char[] c= s.ToCharArray();
                   byte[] b = new byte[c.Length];
                    for(int i=0;i<c.Length;i++)
                    {
                        b[i] = (byte)(((byte)c[i]) - 0x30);
                    } return b;
        }
        static string getUserPassFromHex(byte[] b, uint len)
        {
            string ret = "";
            for (int i = 0; i < len; i++)
            {
                if (b[i] == 0xff)
                    continue;
                ret += (char)(b[i]+0x30);
            }

            return ret;
        }
        private static int CloseUsb(int comHandle)
        {  
            Trace.WriteLine("hfReaderCloseUsb");
           int ret = protocol.hfReaderCloseUsb(comHandle);
            Trace.WriteLine("hfReaderCloseUsb ret:" + ret.ToString());
            return ret;
        }

        private static void  GetUID(int comHandle, out int ret, out string uid)
        {
            byte[] pTxFrame;
            byte[] pRxFrame;

            pTxFrame = new byte[0x400];
            pRxFrame = new byte[0x400];

            byte mode = 1;
            ISO14443A_UIDPARAM param = new ISO14443A_UIDPARAM();
            Trace.WriteLine("iso14443AGetUID");
            ret = protocol.iso14443AGetUID(comHandle, 0x0000, 0x0001, mode, ref param, pTxFrame, pRxFrame);
            Trace.WriteLine("iso14443AGetUID ret:" + ret.ToString());
            uid = "";
            if (ret > 0)
            {
                if (param.num > 0)
                {
                    string s = "";
                    for (int num4 = 0; num4 < protocol.HFREADER_ISO14443A_LEN_SIGNAL_UID; num4++)
                    {
                        s = s + param.uid[0].uid[num4].ToString("X").PadLeft(2, '0');
                    }
                    uid = s;
                }
            }

        }

        private static void SetConfig(int comHandle)
        {
            byte[] pTxFrame;
            byte[] pRxFrame;
            HFREADER_CONFIG config;
            string cfg;

            config = new HFREADER_CONFIG();
            pTxFrame = new byte[0x400];
            pRxFrame = new byte[0x400];


            config.afi = 0;
            config.afiCtrl = protocol.HFREADER_CFG_AFI_DISABLE;
            config.baudrate = protocol.HFREADER_CFG_BAUDRATE38400;
            config.beepStatus = protocol.HFREADER_CFG_BUZZER_ENABLE;
            config.cmdMode = protocol.HFREADER_CFG_INVENTORY_TRIGGER;
            config.readerAddr = 0x0001;
            config.tagStatus = protocol.HFREADER_CFG_QUERY_DISABLE;
            config.uidSendMode = protocol.HFREADER_CFG_UID_POSITIVE;
            config.workMode = protocol.HFREADER_CFG_WM_INVENTORY | (protocol.HFREADER_CFG_TYPE_ISO14443A << 4);
            cfg = string.Format("workMode={0},"
            + "readerAddr={1},"
            + " cmdMode={2},"
            + " afiCtrl={3},"
            + " uidSendMode={4},"
            + " tagStatus={5},"
            + " baudrate={6},"
            + " beepStatus={7},"
            + " afi={8},"
            , config.workMode
            , config.readerAddr
            , config.cmdMode
            , config.afiCtrl
            , config.uidSendMode
            , config.tagStatus
            , config.baudrate
            , config.beepStatus
            , config.afi);
            Trace.WriteLine("hfReaderSetConfig config:" + cfg.ToString());
            int retsetconfig = protocol.hfReaderSetConfig(comHandle, 0, 1, ref config, pTxFrame, pRxFrame);
            Trace.WriteLine("hfReaderSetConfig ret:" + retsetconfig.ToString() + ", config:" + cfg.ToString());

        }

        private static void GetConfig(int comHandle)
        {
            byte[] pTxFrame;
            byte[] pRxFrame;
            HFREADER_CONFIG config;
            string cfg;

            pTxFrame = new byte[0x400];
            pRxFrame = new byte[0x400];

            config = new HFREADER_CONFIG();
            Trace.WriteLine("hfReaderGetConfig");
            int retc = protocol.hfReaderGetConfig(comHandle, 0x0000, 0x0001, ref config, pTxFrame, pRxFrame);
            cfg = string.Format("workMode={0},"
                            + "readerAddr={1},"
                            + " cmdMode={2},"
                            + " afiCtrl={3},"
                            + " uidSendMode={4},"
                            + " tagStatus={5},"
                            + " baudrate={6},"
                            + " beepStatus={7},"
                            + " afi={8},"
                            , config.workMode
                            , config.readerAddr
                            , config.cmdMode
                            , config.afiCtrl
                            , config.uidSendMode
                            , config.tagStatus
                            , config.baudrate
                            , config.beepStatus
                            , config.afi);
            Trace.WriteLine("hfReaderGetConfig ret:" + retc.ToString() + ", config:" + cfg.ToString());
        }

        private static int OpenUsb()
        {
            Trace.WriteLine("hfReaderOpenUsb");
            int comHandle = protocol.hfReaderOpenUsb(0x0505, 0x5050);
            Trace.WriteLine("hfReaderOpenUsb h:" + comHandle.ToString());
            return comHandle;
        }


    }


}
