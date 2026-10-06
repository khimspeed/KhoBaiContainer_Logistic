USE [master]
GO
IF DB_ID(N'KhoBaiContainer') IS NULL
    CREATE DATABASE [KhoBaiContainer];
GO
USE [KhoBaiContainer]
GO

/* ========== 1. Nền tảng: người dùng ========== */
IF OBJECT_ID(N'dbo.[user]', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[user](
        user_id     VARCHAR(50)    NOT NULL,
        hoten       NVARCHAR(150)  NULL,
        ngaysinh    DATE           NULL,
        diachi      NVARCHAR(250)  NULL,
        gioitinh    NVARCHAR(30)   NULL,
        email       VARCHAR(150)   NULL,
        taikhoan    VARCHAR(30)    NULL,
        matkhau     VARCHAR(60)    NULL,
        role        VARCHAR(30)    NULL,
        image_url   VARCHAR(300)   NULL,
        created_at  DATETIME       NOT NULL CONSTRAINT DF_user_created DEFAULT (GETDATE()),
        is_deleted  BIT            NOT NULL CONSTRAINT DF_user_deleted DEFAULT (0),
        CONSTRAINT PK_user PRIMARY KEY (user_id)
    );
    CREATE UNIQUE INDEX UX_user_taikhoan ON dbo.[user](taikhoan) WHERE taikhoan IS NOT NULL;
END
GO

/* ========== 2. Khách hàng ========== */
IF OBJECT_ID(N'dbo.customers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.customers(
        customer_id   VARCHAR(50)    NOT NULL,
        customer_code VARCHAR(30)    NOT NULL,
        customer_name NVARCHAR(200)  NOT NULL,
        tax_code      VARCHAR(30)    NULL,
        phone         VARCHAR(30)    NULL,
        email         VARCHAR(150)   NULL,
        address       NVARCHAR(300)  NULL,
        is_deleted    BIT            NOT NULL CONSTRAINT DF_customers_deleted DEFAULT (0),
        CONSTRAINT PK_customers PRIMARY KEY (customer_id)
    );
END
GO

/* ========== 3. Kho / zone / slot ========== */
IF OBJECT_ID(N'dbo.warehouses', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.warehouses(
        warehouse_id   VARCHAR(50)    NOT NULL,
        warehouse_code VARCHAR(30)    NOT NULL,
        warehouse_name NVARCHAR(200)  NOT NULL,
        address        NVARCHAR(300)  NULL,
        capacity_teu   DECIMAL(18,2)  NOT NULL CONSTRAINT DF_wh_cap DEFAULT (0),
        used_teu       DECIMAL(18,2)  NOT NULL CONSTRAINT DF_wh_used DEFAULT (0),
        is_deleted     BIT            NOT NULL CONSTRAINT DF_wh_deleted DEFAULT (0),
        CONSTRAINT PK_warehouses PRIMARY KEY (warehouse_id)
    );
END
GO

IF OBJECT_ID(N'dbo.warehouse_zones', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.warehouse_zones(
        zone_id      VARCHAR(50)    NOT NULL,
        warehouse_id VARCHAR(50)    NOT NULL,
        zone_code    VARCHAR(30)    NOT NULL,
        zone_name    NVARCHAR(150)  NOT NULL,
        CONSTRAINT PK_warehouse_zones PRIMARY KEY (zone_id),
        CONSTRAINT FK_zones_warehouse FOREIGN KEY (warehouse_id) REFERENCES dbo.warehouses(warehouse_id)
    );
END
GO

IF OBJECT_ID(N'dbo.warehouse_slots', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.warehouse_slots(
        slot_id     VARCHAR(50)   NOT NULL,
        zone_id     VARCHAR(50)   NOT NULL,
        slot_code   VARCHAR(30)   NOT NULL,
        max_teu     DECIMAL(10,2) NOT NULL CONSTRAINT DF_slot_teu DEFAULT (1),
        is_occupied BIT           NOT NULL CONSTRAINT DF_slot_occ DEFAULT (0),
        CONSTRAINT PK_warehouse_slots PRIMARY KEY (slot_id),
        CONSTRAINT FK_slots_zone FOREIGN KEY (zone_id) REFERENCES dbo.warehouse_zones(zone_id)
    );
END
GO

/* ========== 4. Danh mục container / loại hàng ========== */
IF OBJECT_ID(N'dbo.container_types', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.container_types(
        type_id    VARCHAR(50)    NOT NULL,
        type_code  VARCHAR(20)    NOT NULL,
        type_name  NVARCHAR(150)  NOT NULL,
        teu        DECIMAL(10,2)  NOT NULL,
        CONSTRAINT PK_container_types PRIMARY KEY (type_id)
    );
END
GO

IF OBJECT_ID(N'dbo.cargo_types', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.cargo_types(
        cargo_type_id VARCHAR(50)    NOT NULL,
        cargo_code    VARCHAR(20)    NOT NULL,
        cargo_name    NVARCHAR(150)  NOT NULL,
        need_reefer   BIT            NOT NULL CONSTRAINT DF_cargo_reefer DEFAULT (0),
        CONSTRAINT PK_cargo_types PRIMARY KEY (cargo_type_id)
    );
END
GO

/* ========== 5. Container ========== */
IF OBJECT_ID(N'dbo.containers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.containers(
        container_id     VARCHAR(50)    NOT NULL,
        container_no     VARCHAR(20)    NOT NULL,
        type_id          VARCHAR(50)    NOT NULL,
        cargo_type_id    VARCHAR(50)    NULL,
        warehouse_id     VARCHAR(50)    NULL,
        slot_id          VARCHAR(50)    NULL,
        customer_id      VARCHAR(50)    NULL,
        [status]         VARCHAR(30)    NOT NULL CONSTRAINT DF_ctn_status DEFAULT ('Empty'),
        check_in_at      DATETIME       NULL,
        free_until       DATETIME       NULL,
        is_overdue       BIT            NOT NULL CONSTRAINT DF_ctn_overdue DEFAULT (0),
        is_deleted       BIT            NOT NULL CONSTRAINT DF_ctn_deleted DEFAULT (0),
        CONSTRAINT PK_containers PRIMARY KEY (container_id),
        CONSTRAINT UQ_containers_no UNIQUE (container_no),
        CONSTRAINT FK_ctn_type FOREIGN KEY (type_id) REFERENCES dbo.container_types(type_id),
        CONSTRAINT FK_ctn_cargo FOREIGN KEY (cargo_type_id) REFERENCES dbo.cargo_types(cargo_type_id),
        CONSTRAINT FK_ctn_wh FOREIGN KEY (warehouse_id) REFERENCES dbo.warehouses(warehouse_id),
        CONSTRAINT FK_ctn_slot FOREIGN KEY (slot_id) REFERENCES dbo.warehouse_slots(slot_id),
        CONSTRAINT FK_ctn_customer FOREIGN KEY (customer_id) REFERENCES dbo.customers(customer_id)
    );
END
GO

IF OBJECT_ID(N'dbo.container_movements', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.container_movements(
        movement_id     VARCHAR(50)    NOT NULL,
        container_id    VARCHAR(50)    NOT NULL,
        warehouse_id    VARCHAR(50)    NULL,
        from_slot_id    VARCHAR(50)    NULL,
        to_slot_id      VARCHAR(50)    NULL,
        movement_type   VARCHAR(20)    NOT NULL, -- IN / OUT / RELOCATE
        moved_at        DATETIME       NOT NULL CONSTRAINT DF_move_at DEFAULT (GETDATE()),
        created_by      VARCHAR(50)    NULL,
        note            NVARCHAR(300)  NULL,
        CONSTRAINT PK_container_movements PRIMARY KEY (movement_id),
        CONSTRAINT FK_move_ctn FOREIGN KEY (container_id) REFERENCES dbo.containers(container_id),
        CONSTRAINT FK_move_wh FOREIGN KEY (warehouse_id) REFERENCES dbo.warehouses(warehouse_id)
    );
END
GO

/* ========== 6. Phương tiện ========== */
IF OBJECT_ID(N'dbo.vehicles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.vehicles(
        vehicle_id    VARCHAR(50)    NOT NULL,
        plate_no      VARCHAR(20)    NOT NULL,
        vehicle_type  NVARCHAR(50)   NULL,
        driver_name   NVARCHAR(150)  NULL,
        phone         VARCHAR(30)    NULL,
        [status]      VARCHAR(20)    NOT NULL CONSTRAINT DF_veh_status DEFAULT ('Available'),
        CONSTRAINT PK_vehicles PRIMARY KEY (vehicle_id)
    );
END
GO

IF OBJECT_ID(N'dbo.vessels', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.vessels(
        vessel_id   VARCHAR(50)    NOT NULL,
        vessel_code VARCHAR(30)    NOT NULL,
        vessel_name NVARCHAR(200)  NOT NULL,
        imo_no      VARCHAR(20)    NULL,
        CONSTRAINT PK_vessels PRIMARY KEY (vessel_id)
    );
END
GO

/* ========== 7. Vận tải ========== */
IF OBJECT_ID(N'dbo.shipments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.shipments(
        shipment_id   VARCHAR(50)    NOT NULL,
        tracking_no   VARCHAR(40)    NOT NULL,
        customer_id   VARCHAR(50)    NULL,
        origin        NVARCHAR(200)  NULL,
        destination   NVARCHAR(200)  NULL,
        [status]      VARCHAR(30)    NOT NULL CONSTRAINT DF_shp_status DEFAULT ('Planned'),
        eta           DATETIME       NULL,
        created_at    DATETIME       NOT NULL CONSTRAINT DF_shp_created DEFAULT (GETDATE()),
        is_deleted    BIT            NOT NULL CONSTRAINT DF_shp_deleted DEFAULT (0),
        CONSTRAINT PK_shipments PRIMARY KEY (shipment_id),
        CONSTRAINT UQ_shipments_tracking UNIQUE (tracking_no),
        CONSTRAINT FK_shp_customer FOREIGN KEY (customer_id) REFERENCES dbo.customers(customer_id)
    );
END
GO

IF OBJECT_ID(N'dbo.shipment_legs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.shipment_legs(
        leg_id        VARCHAR(50)    NOT NULL,
        shipment_id   VARCHAR(50)    NOT NULL,
        container_id  VARCHAR(50)    NULL,
        vehicle_id    VARCHAR(50)    NULL,
        vessel_id     VARCHAR(50)    NULL,
        sequence_no   INT            NOT NULL,
        mode          VARCHAR(20)    NOT NULL, -- TRUCK / VESSEL
        [status]      VARCHAR(30)    NOT NULL CONSTRAINT DF_leg_status DEFAULT ('Planned'),
        departed_at   DATETIME       NULL,
        arrived_at    DATETIME       NULL,
        CONSTRAINT PK_shipment_legs PRIMARY KEY (leg_id),
        CONSTRAINT FK_leg_shp FOREIGN KEY (shipment_id) REFERENCES dbo.shipments(shipment_id),
        CONSTRAINT FK_leg_ctn FOREIGN KEY (container_id) REFERENCES dbo.containers(container_id),
        CONSTRAINT FK_leg_veh FOREIGN KEY (vehicle_id) REFERENCES dbo.vehicles(vehicle_id),
        CONSTRAINT FK_leg_vsl FOREIGN KEY (vessel_id) REFERENCES dbo.vessels(vessel_id)
    );
END
GO

/* ========== 8. Hợp đồng / hóa đơn / thanh toán ========== */
IF OBJECT_ID(N'dbo.contracts', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.contracts(
        contract_id     VARCHAR(50)     NOT NULL,
        contract_no     VARCHAR(40)     NOT NULL,
        customer_id     VARCHAR(50)     NOT NULL,
        warehouse_id    VARCHAR(50)     NOT NULL,
        start_date      DATE            NOT NULL,
        end_date        DATE            NOT NULL,
        storage_rate    DECIMAL(18,2)   NOT NULL,
        [status]        VARCHAR(20)     NOT NULL CONSTRAINT DF_ctr_status DEFAULT ('Active'),
        is_deleted      BIT             NOT NULL CONSTRAINT DF_ctr_deleted DEFAULT (0),
        CONSTRAINT PK_contracts PRIMARY KEY (contract_id),
        CONSTRAINT UQ_contracts_no UNIQUE (contract_no),
        CONSTRAINT FK_ctr_customer FOREIGN KEY (customer_id) REFERENCES dbo.customers(customer_id),
        CONSTRAINT FK_ctr_wh FOREIGN KEY (warehouse_id) REFERENCES dbo.warehouses(warehouse_id)
    );
END
GO

IF OBJECT_ID(N'dbo.invoices', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.invoices(
        invoice_id    VARCHAR(50)    NOT NULL,
        invoice_no    VARCHAR(40)    NOT NULL,
        contract_id   VARCHAR(50)    NULL,
        customer_id   VARCHAR(50)    NOT NULL,
        invoice_date  DATE           NOT NULL,
        due_date      DATE           NULL,
        amount        DECIMAL(18,2)  NOT NULL,
        [status]      VARCHAR(20)    NOT NULL CONSTRAINT DF_inv_status DEFAULT ('Unpaid'),
        is_deleted    BIT            NOT NULL CONSTRAINT DF_inv_deleted DEFAULT (0),
        CONSTRAINT PK_invoices PRIMARY KEY (invoice_id),
        CONSTRAINT UQ_invoices_no UNIQUE (invoice_no),
        CONSTRAINT FK_inv_contract FOREIGN KEY (contract_id) REFERENCES dbo.contracts(contract_id),
        CONSTRAINT FK_inv_customer FOREIGN KEY (customer_id) REFERENCES dbo.customers(customer_id)
    );
END
GO

IF OBJECT_ID(N'dbo.payments', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.payments(
        payment_id    VARCHAR(50)    NOT NULL,
        invoice_id    VARCHAR(50)    NOT NULL,
        paid_at       DATETIME       NOT NULL CONSTRAINT DF_pay_at DEFAULT (GETDATE()),
        amount        DECIMAL(18,2)  NOT NULL,
        method        VARCHAR(30)    NULL,
        note          NVARCHAR(300)  NULL,
        CONSTRAINT PK_payments PRIMARY KEY (payment_id),
        CONSTRAINT FK_pay_invoice FOREIGN KEY (invoice_id) REFERENCES dbo.invoices(invoice_id)
    );
END
GO

/* ========== 9. IoT / audit / thông báo ========== */
IF OBJECT_ID(N'dbo.iot_readings', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.iot_readings(
        reading_id    VARCHAR(50)    NOT NULL,
        container_id  VARCHAR(50)    NOT NULL,
        temperature   DECIMAL(8,2)   NULL,
        humidity      DECIMAL(8,2)   NULL,
        recorded_at   DATETIME       NOT NULL CONSTRAINT DF_iot_at DEFAULT (GETDATE()),
        CONSTRAINT PK_iot_readings PRIMARY KEY (reading_id),
        CONSTRAINT FK_iot_ctn FOREIGN KEY (container_id) REFERENCES dbo.containers(container_id)
    );
    CREATE INDEX IX_iot_container_time ON dbo.iot_readings(container_id, recorded_at DESC);
END
GO

IF OBJECT_ID(N'dbo.audit_logs', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.audit_logs(
        log_id      VARCHAR(50)     NOT NULL,
        user_id     VARCHAR(50)     NULL,
        action_name VARCHAR(80)     NOT NULL,
        entity_name VARCHAR(80)     NULL,
        entity_id   VARCHAR(50)     NULL,
        created_at  DATETIME        NOT NULL CONSTRAINT DF_audit_at DEFAULT (GETDATE()),
        ip_address  VARCHAR(50)     NULL,
        CONSTRAINT PK_audit_logs PRIMARY KEY (log_id)
    );
END
GO

IF OBJECT_ID(N'dbo.notifications', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.notifications(
        notification_id VARCHAR(50)    NOT NULL,
        title           NVARCHAR(200)  NOT NULL,
        content         NVARCHAR(1000) NULL,
        user_id         VARCHAR(50)    NULL,
        is_read         BIT            NOT NULL CONSTRAINT DF_noti_read DEFAULT (0),
        created_at      DATETIME       NOT NULL CONSTRAINT DF_noti_at DEFAULT (GETDATE()),
        CONSTRAINT PK_notifications PRIMARY KEY (notification_id)
    );
END
GO

/* ========== 10. Dữ liệu mẫu ========== */
IF NOT EXISTS (SELECT 1 FROM dbo.[user] WHERE taikhoan = 'admin')
BEGIN
    INSERT INTO dbo.[user](user_id, hoten, ngaysinh, diachi, gioitinh, email, taikhoan, matkhau, role)
    VALUES
    ('u-admin', N'Administrator', '1977-01-08', N'Hà Nội', N'Nam', 'admin@khobai.local', 'admin', 'admin', 'Admin'),
    ('u-dispatch', N'Điều phối viên', '1990-05-15', N'Hải Phòng', N'Nam', 'dispatcher@khobai.local', 'dispatcher', '123456', 'Dispatcher'),
    ('u-warehouse', N'Nhân viên kho', '1995-08-20', N'Hải Phòng', N'Nữ', 'warehouse@khobai.local', 'warehouse', '123456', 'WarehouseStaff');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.customers)
BEGIN
    INSERT INTO dbo.customers(customer_id, customer_code, customer_name, tax_code, phone, email, address)
    VALUES
    ('cus-001', 'KH001', N'Công ty Logistics Hải Phòng', '0101234567', '0225123456', 'hp@logi.vn', N'Hải Phòng'),
    ('cus-002', 'KH002', N'Công ty Vận tải Biển Đông', '0207654321', '0287654321', 'bd@ship.vn', N'Quận 1, TP.HCM');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.warehouses)
BEGIN
    INSERT INTO dbo.warehouses(warehouse_id, warehouse_code, warehouse_name, address, capacity_teu, used_teu)
    VALUES
    ('wh-001', 'KHO-HP', N'Kho bãi Hải Phòng', N'Cảng Hải Phòng', 2000, 4),
    ('wh-002', 'KHO-HCM', N'Kho bãi Cát Lái', N'Cảng Cát Lái', 3500, 0);

    INSERT INTO dbo.warehouse_zones(zone_id, warehouse_id, zone_code, zone_name)
    VALUES
    ('zn-a', 'wh-001', 'A', N'Khu A - Dry'),
    ('zn-b', 'wh-001', 'B', N'Khu B - Reefer'),
    ('zn-c', 'wh-002', 'C', N'Khu C - Dry');

    INSERT INTO dbo.warehouse_slots(slot_id, zone_id, slot_code, max_teu, is_occupied)
    VALUES
    ('sl-a01', 'zn-a', 'A-01', 1, 1),
    ('sl-a02', 'zn-a', 'A-02', 1, 1),
    ('sl-b01', 'zn-b', 'B-01', 1, 0),
    ('sl-c01', 'zn-c', 'C-01', 1, 0);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.container_types)
BEGIN
    INSERT INTO dbo.container_types(type_id, type_code, type_name, teu)
    VALUES
    ('ct-20', '20GP', N'Container 20 feet khô', 1),
    ('ct-40', '40GP', N'Container 40 feet khô', 2),
    ('ct-40r', '40RF', N'Container 40 feet lạnh', 2);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.cargo_types)
BEGIN
    INSERT INTO dbo.cargo_types(cargo_type_id, cargo_code, cargo_name, need_reefer)
    VALUES
    ('cg-dry', 'DRY', N'Hàng khô', 0),
    ('cg-food', 'FOOD', N'Thực phẩm đông lạnh', 1),
    ('cg-dg', 'DG', N'Hàng nguy hiểm', 0);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.containers)
BEGIN
    INSERT INTO dbo.containers(container_id, container_no, type_id, cargo_type_id, warehouse_id, slot_id, customer_id, [status], check_in_at, free_until, is_overdue)
    VALUES
    ('cn-001', 'MSCU1234567', 'ct-20', 'cg-dry', 'wh-001', 'sl-a01', 'cus-001', 'InYard', DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, -2, GETDATE()), 1),
    ('cn-002', 'TEMU7654321', 'ct-40', 'cg-dry', 'wh-001', 'sl-a02', 'cus-002', 'InYard', DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, 4, GETDATE()), 0),
    ('cn-003', 'HLCU9988776', 'ct-40r', 'cg-food', NULL, NULL, 'cus-001', 'InTransit', NULL, NULL, 0);

    INSERT INTO dbo.container_movements(movement_id, container_id, warehouse_id, to_slot_id, movement_type, created_by, note)
    VALUES
    ('mv-001', 'cn-001', 'wh-001', 'sl-a01', 'IN', 'u-warehouse', N'Nhập bãi'),
    ('mv-002', 'cn-002', 'wh-001', 'sl-a02', 'IN', 'u-warehouse', N'Nhập bãi');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.vehicles)
BEGIN
    INSERT INTO dbo.vehicles(vehicle_id, plate_no, vehicle_type, driver_name, phone, [status])
    VALUES
    ('vh-001', '15C-123.45', N'Đầu kéo', N'Nguyễn Văn Tài', '0901111222', 'Busy'),
    ('vh-002', '15C-678.90', N'Xe tải', N'Trần Văn Lái', '0903333444', 'Available');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.vessels)
BEGIN
    INSERT INTO dbo.vessels(vessel_id, vessel_code, vessel_name, imo_no)
    VALUES ('vs-001', 'VSL01', N'MV Bien Dong Star', 'IMO9123456');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.shipments)
BEGIN
    INSERT INTO dbo.shipments(shipment_id, tracking_no, customer_id, origin, destination, [status], eta)
    VALUES
    ('sh-001', 'TRK-2026-0001', 'cus-001', N'Cảng Hải Phòng', N'Kho KHO-HP', 'Arrived', GETDATE()),
    ('sh-002', 'TRK-2026-0002', 'cus-001', N'Cảng Cát Lái', N'Kho KHO-HP', 'InTransit', DATEADD(DAY, 1, GETDATE()));

    INSERT INTO dbo.shipment_legs(leg_id, shipment_id, container_id, vehicle_id, sequence_no, mode, [status], arrived_at)
    VALUES
    ('lg-001', 'sh-001', 'cn-001', 'vh-001', 1, 'TRUCK', 'Delivered', GETDATE()),
    ('lg-002', 'sh-002', 'cn-003', 'vh-001', 1, 'TRUCK', 'InTransit', NULL);
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.contracts)
BEGIN
    INSERT INTO dbo.contracts(contract_id, contract_no, customer_id, warehouse_id, start_date, end_date, storage_rate, [status])
    VALUES
    ('ct-001', 'HD-2026-001', 'cus-001', 'wh-001', '2026-01-01', '2026-12-31', 150000, 'Active');

    INSERT INTO dbo.invoices(invoice_id, invoice_no, contract_id, customer_id, invoice_date, due_date, amount, [status])
    VALUES
    ('inv-001', 'INV-2026-001', 'ct-001', 'cus-001', CAST(GETDATE() AS DATE), DATEADD(DAY, 15, GETDATE()), 3000000, 'Unpaid');
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.iot_readings)
BEGIN
    INSERT INTO dbo.iot_readings(reading_id, container_id, temperature, humidity)
    VALUES
    ('iot-001', 'cn-003', 2.5, 78.0),
    ('iot-002', 'cn-003', 3.1, 80.2);
END
GO

/* ========== 11. Stored procedures User (API đang dùng) ========== */
CREATE OR ALTER PROCEDURE dbo.sp_user_create
    @user_id VARCHAR(50),
    @hoten NVARCHAR(150),
    @ngaysinh DATE,
    @diachi NVARCHAR(250),
    @gioitinh NVARCHAR(30),
    @email VARCHAR(150),
    @taikhoan VARCHAR(30),
    @matkhau VARCHAR(60),
    @role VARCHAR(30),
    @image_url VARCHAR(300)
AS
BEGIN
    INSERT INTO dbo.[user](user_id, hoten, ngaysinh, diachi, gioitinh, email, taikhoan, matkhau, role, image_url)
    VALUES (@user_id, @hoten, @ngaysinh, @diachi, @gioitinh, @email, @taikhoan, @matkhau, @role, @image_url);
    SELECT '';
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_user_update
    @user_id VARCHAR(50),
    @hoten NVARCHAR(150),
    @ngaysinh DATE,
    @diachi NVARCHAR(250),
    @gioitinh NVARCHAR(30),
    @email VARCHAR(150),
    @taikhoan VARCHAR(30),
    @matkhau VARCHAR(60),
    @role VARCHAR(30),
    @image_url VARCHAR(300)
AS
BEGIN
    UPDATE dbo.[user]
    SET hoten = @hoten, ngaysinh = @ngaysinh, diachi = @diachi, gioitinh = @gioitinh,
        email = @email, taikhoan = @taikhoan, matkhau = @matkhau, role = @role, image_url = @image_url
    WHERE user_id = @user_id AND is_deleted = 0;
    SELECT '';
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_user_delete
    @user_id VARCHAR(50)
AS
BEGIN
    UPDATE dbo.[user] SET is_deleted = 1 WHERE user_id = @user_id;
    SELECT '';
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_user_get_by_id
    @user_id VARCHAR(50)
AS
BEGIN
    SELECT user_id, hoten, ngaysinh, diachi, gioitinh, email, taikhoan, matkhau, role, image_url
    FROM dbo.[user]
    WHERE user_id = @user_id AND is_deleted = 0;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_user_get_by_username_password
    @taikhoan VARCHAR(30),
    @matkhau VARCHAR(60)
AS
BEGIN
    SELECT user_id, hoten, ngaysinh, diachi, gioitinh, email, taikhoan, role, image_url
    FROM dbo.[user]
    WHERE taikhoan = @taikhoan AND matkhau = @matkhau AND is_deleted = 0;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_user_search
    @page_index INT,
    @page_size INT,
    @hoten NVARCHAR(150),
    @taikhoan VARCHAR(30)
AS
BEGIN
    DECLARE @RecordCount BIGINT;

    SELECT ROW_NUMBER() OVER (ORDER BY u.hoten ASC) AS RowNumber,
           u.user_id, u.hoten, u.ngaysinh, u.diachi, u.gioitinh,
           u.email, u.taikhoan, u.matkhau, u.role, u.image_url
    INTO #Results
    FROM dbo.[user] AS u
    WHERE u.is_deleted = 0
      AND u.taikhoan <> 'admin'
      AND ((@hoten = '') OR (u.hoten LIKE N'%' + @hoten + N'%'))
      AND ((@taikhoan = '') OR (u.taikhoan = @taikhoan));

    SELECT @RecordCount = COUNT(*) FROM #Results;

    IF (@page_size <> 0 AND @page_index <> -1)
        SELECT *, @RecordCount AS RecordCount
        FROM #Results
        WHERE RowNumber BETWEEN (@page_index - 1) * @page_size + 1
                            AND (@page_index * @page_size);
    ELSE
        SELECT *, @RecordCount AS RecordCount FROM #Results;

    DROP TABLE #Results;
END
GO
