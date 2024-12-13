# Crimson Throne - Đồ án môn học "Phát triển game"

## Thành viên thực hiện
Nguyễn Nhật Khoa

Trần Anh Kiệt

Nguyễn Hà Hoàng Tuấn

## Cấu trúc thư mục
```
Assets/
|-- _ProjectSettings/             # Thư mục mặc định của Unity (tự động tạo)
|
|-- Art/                          # Chứa các assets đồ họa
|   |-- Tiles/                    # Tilemap cho các map
|   |   |-- Map1/                 # Map 1 
|   |   |-- Map2/                 # Map 2 
|   |   |-- Map3/                 # Map 3 
|   |-- Characters/               # Nhân vật & Quái vật
|   |   |-- Player/               # Animation cho Player
|   |   |   |-- Knight/           # Hiệp sĩ
|   |   |   |-- Assassin/         # Thích khách
|   |   |   |-- Mage/             # Pháp sư
|   |   |-- Enemies/              # Animation cho Quái
|   |   |   |-- Skeleton/     
|   |   |   |-- Assassin/     
|   |   |   |-- LizardDemon/  
|   |   |   |-- ...
|   |-- UI/                       # UI game (Main Menu, HUD)
|
|-- Prefabs/                      # Chứa các prefab được tái sử dụng
|   |-- Player/                   # Prefab cho Player
|   |-- Enemies/                  # Prefab cho Quái
|   |-- Tiles/                    # Prefab Tilemap
|   |-- UI/                       # Prefab UI
|
|-- Scripts/                      # Tất cả các scripts
|   |-- Player/                   # Scripts liên quan đến Player
|   |   |-- PlayerController.cs   # Điều khiển input
|   |   |-- PlayerAnimation.cs    # Điều khiển animation
|   |-- Enemies/                  # Scripts điều khiển Quái
|   |   |-- EnemyBase.cs          # Script cha cho tất cả quái
|   |   |-- EnemyMovement.cs      # Điều khiển di chuyển của quái
|   |-- Managers/                 # Scripts quản lý chung
|   |   |-- GameManager.cs        # Quản lý trạng thái game
|   |   |-- UIManager.cs          # Quản lý UI
|   |-- Utilities/                # Scripts tiện ích (helper)
|
|-- Scenes/                       # Chứa các scene của game
|   |-- MainMenu.unity            # Scene Main Menu
|   |-- Map1.unity                # Scene Map 1
|   |-- Map2.unity                # Scene Map 2
|   |-- Map3.unity                # Scene Map 3
|
|-- Audio/                        # Chứa nhạc nền và âm thanh
|   |-- BGM/                      # Nhạc nền (Background Music)
|   |-- SFX/                      # Hiệu ứng âm thanh (Sound Effects)
|
|-- Fonts/                        # Chứa các font chữ sử dụng trong game
|
|-- Materials/                    # Chứa các material cho đồ họa
|
|-- Animations/                   # Chứa animation clips và animator controllers
|   |-- Player/                   # Animation cho Player
|   |-- Enemies/                  # Animation cho Quái
|
|-- Plugins/                      # Các plugin hoặc package thêm
|
|-- Documentation/                # Tài liệu liên quan đến dự án
|   |-- ReadMe.txt                # Hướng dẫn cơ bản cho dự án
|
|-- StreamingAssets/              # Dữ liệu như video, file ngoài
|
|-- Resources/                    # Dùng để load assets động (dùng với Resources.Load)
|
|-- Testing/                      # Scripts hoặc scene test chức năng
```

## Mô tả


## Mục tiêu


## Thể loại


## Kĩ thuật sử dụng
UI, va chạm, nhặt item, xử lí item, âm thanh, memu...

## Ghi chú
Toàn bộ code là xử lí logic cho vật thể, có thể lấy tài nguyên để chạy ở link Drive.

## Link tải tài nguyên


## Link video demo
