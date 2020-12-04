<?php

namespace App\Console\Commands\Install;

use Illuminate\Console\Command;

class Update extends Command
{
    /**
     * The name and signature of the console command.
     *
     * @var string
     */
    protected $signature = 'install:update';

    /**
     * The console command description.
     *
     * @var string
     */
    protected $description = 'Execute the actions to perform the system update';

    /**
     * Create a new command instance.
     *
     * @return void
     */
    public function __construct()
    {
        parent::__construct();
    }

    /**
     * Execute the console command.
     *
     * @return mixed
     */
    public function handle()
    {
        $this->call('down');
        // Database tables
        $this->info('Step 1 of 3 - Creating the database tables');
        $this->call('migrate', ['--force' => true]);
        // Database seeding
        $this->info('Step 2 of 3 - Introducing the data in the database');
        $this->call('db:seed', ['--force' => true]);
        // Database seeding
        $this->info('Step 3 of 3 - Update cache');
        $this->call('route:cache');
        $this->call('cache:clear');
        $this->call('config:cache');
        $this->call('optimize:clear');
        $this->call('auth:clear-resets');
        // Success
        file_put_contents(storage_path('framework/cache/installed'), date('Y/m/d h:i:s').PHP_EOL, FILE_APPEND | LOCK_EX);
        $this->call('up');
        $this->info('Update completed successfully');
        return true;
    }
}
