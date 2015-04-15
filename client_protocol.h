#ifndef __CLIENT_PROTOCOL_H__
#define __CLIENT_PROTOCOL_H__

#define CLIENT_DATA_HEAD_FLAG1		0x58
#define CLIENT_DATA_HEAD_FLAG2		0x44

typedef struct proto_head
{
	u8 head_id[2];		//
	u8 ver;
	u16 total_len;
	u8 seriel_num;
	u8 cmd;
	u8 send_addr;
	u8 send_type;
	u8 recv_addr;
	u8 recv_type;		//	
}__attribute__((packed,aligned(1)))TPROTO_HEAD;


typedef struct client_register
{
	TPROTO_HEAD head;
	u8 client_id[16];		//
	u8 client_type;
	u8 client_mac[17];		//	
}__attribute__((packed,aligned(1)))CLIENT_REGISTER;

typedef struct client_register_ack
{
	TPROTO_HEAD head;
	u8 client_id[16];		//
	u8 client_type;
	u8 result;				//	
}__attribute__((packed,aligned(1)))CLIENT_REGISTER_ACK;

typedef struct client_heartbeat
{
	TPROTO_HEAD head;
	u8 client_id[16];		//
	u8 client_type;
}__attribute__((packed,aligned(1)))CLIENT_HB;

typedef struct client_order_state
{
	TPROTO_HEAD head;
	u8 flag;		//
}__attribute__((packed,aligned(1)))CLIENT_ORDER_STATE;


typedef struct client_order_state_ack
{
	TPROTO_HEAD head;
	u8 result;		//
}__attribute__((packed,aligned(1)))CLIENT_ORDER_STATE_ACK;


typedef struct pile_state
{
	TPROTO_HEAD head;
	u8 pile_id[16];		//
	u8 user_id[32];
	u8 online;				//	
	u8 state;
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_STATE_0x84;


typedef struct client_order_charge_rt_data
{
	TPROTO_HEAD head;
	u8 flag;		//
}__attribute__((packed,aligned(1)))CLIENT_CHARGE_RT_DATA_ORDER;


typedef struct client_order_charge_rt_data_ack
{
	TPROTO_HEAD head;
	u8 result;		//
}__attribute__((packed,aligned(1)))CLIENT_ORDER_CHARGE_RT_DATA_ACK;


typedef struct pile_charge_rt_data
{
	TPROTO_HEAD head;
	u8 pile_id[16];		//
	u16 output_vol;
	u16 output_cur;
	u8 relay_sate;
	u8 switch_state;
	u32 total_degree;
	u8 batt_state;
	u16 work_state;
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_CHARGE_RT_DATA_0x86;


typedef struct pile_charge_info
{
	TPROTO_HEAD head;
	u8 pile_id[16];
}__attribute__((packed,aligned(1)))PILE_CHARGE_INFO;

typedef struct pile_charge_info_ack
{
	TPROTO_HEAD head;
	u8 pile_id[16];
	u8 pile_type;
	u16 ac_input_vol_u;
	u16 ac_input_vol_v;
	u16 ac_input_vol_w;
	u16 ac_output_vol_u;
	u16 ac_output_vol_v;
	u16 ac_output_vol_w;
	u16 ac_output_cur;
	u16 dc_output_vol;
	u16 dc_output_cur;
	u32 total_degree;
	u32 taper_degree;
	u32 peak_degree;
	u32 flat_degree;
	u32 valley_degree;
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_CHARGE_INFO_ACK;


typedef struct pile_software_ver
{
	TPROTO_HEAD head;
	u8 pile_id[16];
}__attribute__((packed,aligned(1)))PILE_SOFTWARE_VER;


typedef struct pile_software_ver_ack
{
	TPROTO_HEAD head;	
	u8 pile_id[16];		//
	u8 vender_id[16];
	u8 software_ver[16];
	u16 crc;
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_SOFTWARE_VER_0x88;


typedef struct pile_id_modify
{
	TPROTO_HEAD head;	
	u8 pile_id[16];		//
	u8 new_pile_id[16];		//
}__attribute__((packed,aligned(1)))PILE_ID_MODIFY;


typedef struct pile_id_modify_ack
{
	TPROTO_HEAD head;	
	u8 pile_id[16];		//
	u8 new_pile_id[16];		//
	u8 result;
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_ID_MODIFY_ACK;



//改变充电桩参数结构
typedef struct pile_para_modify
{
	TPROTO_HEAD head;	
	u8 pile_id[16];
	u8 addr_enable;						//设备地址使能(0:无效1:有效)
	u16 addr;							//设备地址(默认填0x0010)
	u8 station_enable;					//站级地址使能(0:无效1:有效)
	u16 station;						//站级地址(默认填0x000A)
	u8 control_enable;					//控制引导使能(0:无效1:有效)
	u8 control;							//控制引导 (0:不使用1:使用)
	u8 lock_enable;						//电子锁使能(0:无效1:有效)
	u8 lock;							//电子锁 (0:全部不用1:门锁用2:插头用3:全部用)
	u8 ratio_enable;					//占空比使能(0:无效1:有效)
	f32 ratio;							//占空比 (精确到小数点后两位)
	u8 passw_enable;					//维护密码使能(0:无效1:有效)
	u8 passw[7];						//维护密码 (0~9ascii码)(6位编码字符串，最后一位补0)
	u8 model_enable;					//账户模式使能(0:无效1:有效)
	u8 model;							//账户模式 (0:本地模式1:账户模式)
	u8 contrast_enable;					//对比度使能(0:无效1:有效)
	u8 contrast;						//对比度 (默认填52)
	u8 backlight_enable;				//背光时间使能(0:无效1:有效)
	u8 backlight;						//背光时间 (单位分钟默认填1分钟)
}__attribute__((packed,aligned(1)))PILE_PARA_MODIFY;



typedef struct pile_para_modify_ack
{
	TPROTO_HEAD head;
	u8 pile_id[16];
	u8 addr_enable;						//设备地址使能(0:成功1:失败)
	u8 station_enable;					//站级地址使能(0:成功1:失败)
	u8 control_enable;					//控制引导使能(0:成功1:失败)
	u8 lock_enable;						//电子锁使能(0:成功1:失败)
	u8 ratio_enable;					//占空比使能(0:成功1:失败)
	u8 passw_enable;					//维护密码使能(0:成功1:失败)
	u8 model_enable;					//账户模式使能(0:成功1:失败)
	u8 contrast_enable;					//对比度使能(0:成功1:失败)
	u8 backlight_enable;				//背光时间使能(0:成功1:失败)
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_PARA_MODIFY_ACK;



//
typedef struct pile_charge_price
{
	TPROTO_HEAD head;	
	u8 pile_id[16];
	u32 taper_price;
	u32 peak_price;
	u32 flat_price;
	u32 valley_price;
}__attribute__((packed,aligned(1)))PILE_CHARGE_PRICE;

//
typedef struct pile_charge_price_ack
{
	TPROTO_HEAD head;	
	u8 pile_id[16];
	u8 result;
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_CHARGE_PRICE_ACK;


//
typedef struct pile_charge_mode_request
{
	TPROTO_HEAD head;	
	u8 pile_id[16];
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_CHARGE_MODE_R;


//计费模型数据消息结构
typedef struct pile_charge_mode
{
	TPROTO_HEAD head;	
	u8 pile_id[16];	
	u8 model_id[8];						//计费模型编号
	u8 valid_time[7];					//生效时间
	u8 invalid_time[7];					//失效时间
	u16 opt_state;						//执行状态(1-有2-无效)
	u16 pay_type;						//计量类型(1-历程2-充电量3-放电量)
	u8 rate_num;						//费率数(<=4,每个费率对应三个时段)
	u8 time_num;						//时段数(<=12)
	u8 time_param[12][3];				//时段参数(单时段1字节小时，2字节分钟，3字节费率号)
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_CHARGE_MODE;


typedef struct pile_charge_mode_ack
{
	TPROTO_HEAD head;	
	u8 pile_id[16];	
	u8 model_sn[8];						//计费模型编号
	u8 result;							//设置计费模型结果(0:成功1:失败)
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_CHARGE_MODE_ACK;



//
typedef struct pile_service_state_modify
{
	TPROTO_HEAD head;	
	u8 pile_id[16];	
	u8 service_state;
}__attribute__((packed,aligned(1)))PILE_SERVICE_STATE_MODIFY;


typedef struct pile_service_state_modify_ack
{
	TPROTO_HEAD head;	
	u8 pile_id[16];	
	u8 service_state;				
	u8 result;			
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_SERVICE_STATE_MODIFY_ACK;


typedef struct _pile_black_list
{
	u8 card_id[16];	
	u8 state;
}__attribute__((packed,aligned(1)))PILE_BLACK_L;

//0x25
typedef struct pile_black_list_down
{
	TPROTO_HEAD head;	
	u8 pile_id[16];
	u8 timestamp[18];
	u8 black_num;
	PILE_BLACK_L black_list[25]; 
}__attribute__((packed,aligned(1)))PILE_BLACK_LIST_DOWN;

//0x25
typedef struct pile_black_list_ack
{
	TPROTO_HEAD head;	
	u8 pile_id[16];	
	u8 timestamp[18];
	u8 result;
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_BLACK_LIST_ACK;


int client_data_0x01_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x02_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x03_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x05_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x07_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x08_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x20_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x21_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x22_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x23_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x24_handler(int skfd, u8 *data_buf, u16 total_len);
int client_data_0x25_handler(int skfd, u8 *data_buf, u16 total_len);

#endif

