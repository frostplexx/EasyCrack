#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")] // hide console window on Windows in release

mod cracker;
use cracker::VideoGame;

use eframe::egui;

fn main() -> Result<(), eframe::Error> {
    env_logger::init(); // Log to stderr (if you run with `RUST_LOG=debug`).

    let options = eframe::NativeOptions {
        viewport: egui::ViewportBuilder::default().with_inner_size([420.0,240.0]),
        ..Default::default()
    };


    let mut game = VideoGame {
        game_path: String::new(),
        app_id: String::new(),
        create_mods_folder: false,
        language: String::new(),
        playername: String::new(),
        game_name: String::new(),
    };

    eframe::run_simple_native("EasyCrack", options, move |ctx, _frame| {
        egui::CentralPanel::default().show(ctx, |ui| {
            ui.heading("EasyCrack");

            ui.horizontal(|ui| {
                ui.vertical(|ui| {
                let game_path_label = ui.label("Path to game .exe:");
                ui.text_edit_singleline(&mut game.game_path)
                    .labelled_by(game_path_label.id);
                });

                ui.button("Browse").on_hover_text("Browse for game .exe");
            });

            ui.horizontal(|ui| {

                ui.vertical(|ui| {
                let game_name_label = ui.label("Search game on Steam:");
                ui.text_edit_singleline(&mut game.game_name)
                    .labelled_by(game_name_label.id);
                });
                ui.button("Search").on_hover_text("Search for game on Steam");
            });

            ui.horizontal(|ui| {
                let app_id_label = ui.label("or enter AppID");
                ui.text_edit_singleline(&mut game.app_id)
                    .labelled_by(app_id_label.id);
            });

            ui.horizontal(|ui| {
                ui.vertical(|ui| {
                    let username_label = ui.label("Player name:");
                    ui.text_edit_singleline(&mut game.playername)
                        .labelled_by(username_label.id);
                });

                ui.vertical(|ui| {
                    let language_label = ui.label("Language");
                    ui.text_edit_singleline(&mut game.language)
                        .labelled_by(language_label.id);
                });

            });

            ui.checkbox(&mut game.create_mods_folder, "Create mods folder");


            ui.horizontal(|ui| {
                if ui.button("Patch").on_hover_text("Download emulator and crack game").clicked() {
                    game.download_emu();
                    game.create_mods_folder();
                    game.unzip_emu();
                    game.crack();
                }
                ui.button("Help").on_hover_text("Help");
            });
            // ui.progress(0.5).text("Downloading emulator");


        });
    })
}
