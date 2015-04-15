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



//�ı���׮�����ṹ
typedef struct pile_para_modify
{
	TPROTO_HEAD head;	
	u8 pile_id[16];
	u8 addr_enable;						//�豸��ַʹ��(0:��Ч1:��Ч)
	u16 addr;							//�豸��ַ(Ĭ����0x0010)
	u8 station_enable;					//վ����ַʹ��(0:��Ч1:��Ч)
	u16 station;						//վ����ַ(Ĭ����0x000A)
	u8 control_enable;					//��������ʹ��(0:��Ч1:��Ч)
	u8 control;							//�������� (0:��ʹ��1:ʹ��)
	u8 lock_enable;						//������ʹ��(0:��Ч1:��Ч)
	u8 lock;							//������ (0:ȫ������1:������2:��ͷ��3:ȫ����)
	u8 ratio_enable;					//ռ�ձ�ʹ��(0:��Ч1:��Ч)
	f32 ratio;							//ռ�ձ� (��ȷ��С�������λ)
	u8 passw_enable;					//ά������ʹ��(0:��Ч1:��Ч)
	u8 passw[7];						//ά������ (0~9ascii��)(6λ�����ַ��������һλ��0)
	u8 model_enable;					//�˻�ģʽʹ��(0:��Ч1:��Ч)
	u8 model;							//�˻�ģʽ (0:����ģʽ1:�˻�ģʽ)
	u8 contrast_enable;					//�Աȶ�ʹ��(0:��Ч1:��Ч)
	u8 contrast;						//�Աȶ� (Ĭ����52)
	u8 backlight_enable;				//����ʱ��ʹ��(0:��Ч1:��Ч)
	u8 backlight;						//����ʱ�� (��λ����Ĭ����1����)
}__attribute__((packed,aligned(1)))PILE_PARA_MODIFY;



typedef struct pile_para_modify_ack
{
	TPROTO_HEAD head;
	u8 pile_id[16];
	u8 addr_enable;						//�豸��ַʹ��(0:�ɹ�1:ʧ��)
	u8 station_enable;					//վ����ַʹ��(0:�ɹ�1:ʧ��)
	u8 control_enable;					//��������ʹ��(0:�ɹ�1:ʧ��)
	u8 lock_enable;						//������ʹ��(0:�ɹ�1:ʧ��)
	u8 ratio_enable;					//ռ�ձ�ʹ��(0:�ɹ�1:ʧ��)
	u8 passw_enable;					//ά������ʹ��(0:�ɹ�1:ʧ��)
	u8 model_enable;					//�˻�ģʽʹ��(0:�ɹ�1:ʧ��)
	u8 contrast_enable;					//�Աȶ�ʹ��(0:�ɹ�1:ʧ��)
	u8 backlight_enable;				//����ʱ��ʹ��(0:�ɹ�1:ʧ��)
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


//�Ʒ�ģ��������Ϣ�ṹ
typedef struct pile_charge_mode
{
	TPROTO_HEAD head;	
	u8 pile_id[16];	
	u8 model_id[8];						//�Ʒ�ģ�ͱ��
	u8 valid_time[7];					//��Чʱ��
	u8 invalid_time[7];					//ʧЧʱ��
	u16 opt_state;						//ִ��״̬(1-��2-��Ч)
	u16 pay_type;						//��������(1-����2-�����3-�ŵ���)
	u8 rate_num;						//������(<=4,ÿ�����ʶ�Ӧ����ʱ��)
	u8 time_num;						//ʱ����(<=12)
	u8 time_param[12][3];				//ʱ�β���(��ʱ��1�ֽ�Сʱ��2�ֽڷ��ӣ�3�ֽڷ��ʺ�)
	u16 check_sum;
}__attribute__((packed,aligned(1)))PILE_CHARGE_MODE;


typedef struct pile_charge_mode_ack
{
	TPROTO_HEAD head;	
	u8 pile_id[16];	
	u8 model_sn[8];						//�Ʒ�ģ�ͱ��
	u8 result;							//���üƷ�ģ�ͽ��(0:�ɹ�1:ʧ��)
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

